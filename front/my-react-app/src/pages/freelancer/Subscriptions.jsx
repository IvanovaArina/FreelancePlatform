// src/pages/freelancer/Subscriptions.jsx
import { useState, useEffect } from 'react';
import { ActionButton, Input, Textarea } from '../../components/ui';
import { subscriptionApi } from '../../api/subscriptionApi';

export default function FreelancerSubscriptions() {
  const [subscriptions, setSubscriptions] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [editingId, setEditingId] = useState(null);
  const [formData, setFormData] = useState({});
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [createForm, setCreateForm] = useState({
    name: '',
    description: '',
    price: '',
    whatIncludes: [''],
    isActive: true,
  });

  const itemsPerPage = 3;

  // Загрузка подписок
  useEffect(() => {
    loadSubscriptions();
  }, []);

  const loadSubscriptions = async () => {
    try {
      const res = await subscriptionApi.getMySubscriptions();
      setSubscriptions(res.data);
    } catch (err) {
      console.error('Ошибка загрузки подписок:', err);
      alert('Не удалось загрузить подписки');
    }
  };

  const totalPages = Math.ceil(subscriptions.length / itemsPerPage);
  const startIndex = (currentPage - 1) * itemsPerPage;
  const currentSubscriptions = subscriptions.slice(startIndex, startIndex + itemsPerPage);

  // === Редактирование ===
  const handleEdit = (sub) => {
    setEditingId(sub.id);
    setFormData({
      name: sub.name,
      description: sub.description,
      price: sub.price,
      whatIncludes: sub.whatIncludes || [],
      isActive: sub.isActive,
    });
  };

  const handleSave = async () => {
    try {
      await subscriptionApi.updateSubscription(editingId, {
        name: formData.name,
        description: formData.description,
        price: formData.price,
        isActive: formData.isActive,
        whatIncludes: formData.whatIncludes,
      });
      loadSubscriptions();
      setEditingId(null);
      alert('Subscription updated!');
    } catch {
      alert('Failed to update subscription');
    }
  };

  const handleCancel = () => {
    setEditingId(null);
    setFormData({});
  };

  // === Удаление ===
  const handleDelete = async (id) => {
    if (!confirm('Delete this subscription?')) return;
    try {
      await subscriptionApi.deleteSubscription(id);
      loadSubscriptions();
      alert('Subscription deleted');
    } catch {
      alert('Failed to delete');
    }
  };

  // === Создание ===
  const handleCreateSave = async () => {
    if (!createForm.name || !createForm.price) {
      alert('Name and price are required');
      return;
    }
    try {
      await subscriptionApi.createSubscription({
        name: createForm.name,
        description: createForm.description,
        price: createForm.price,
        whatIncludes: createForm.whatIncludes.filter(i => i.trim()),
      });
      setShowCreateModal(false);
      loadSubscriptions();
      alert('Subscription created!');
    } catch {
      alert('Failed to create subscription');
    }
  };

  const addInclude = () => {
    setCreateForm(prev => ({ ...prev, whatIncludes: [...prev.whatIncludes, ''] }));
  };

  const removeInclude = (index) => {
    setCreateForm(prev => ({
      ...prev,
      whatIncludes: prev.whatIncludes.filter((_, i) => i !== index),
    }));
  };

  const updateInclude = (index, value) => {
    const newIncludes = [...createForm.whatIncludes];
    newIncludes[index] = value;
    setCreateForm(prev => ({ ...prev, whatIncludes: newIncludes }));
  };

  // === Подсчёт аналитики ===
  const totalSubscribers = subscriptions.filter(s => s.isActive).length * 3; // пример
  const monthlyRevenue = subscriptions
    .filter(s => s.isActive)
    .reduce((sum, s) => sum + parseFloat(s.price.replace(/[^0-9.]/g, '') || 0), 0);
  const annualRevenue = monthlyRevenue * 12;

  return (
    <div className="max-w-6xl mx-auto space-y-8 pb-12">

      {/* Заголовок */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">My Subscriptions</h1>
          <p className="text-sm text-gray-600">Create and manage subscription-based services</p>
        </div>
        <ActionButton onClick={() => setShowCreateModal(true)}>Create Subscription</ActionButton>
      </div>

      {/* Список подписок */}
      <div className="space-y-6">
        {currentSubscriptions.length === 0 ? (
          <p className="text-center text-gray-500 py-12">No subscriptions yet. Create your first one!</p>
        ) : (
          currentSubscriptions.map((sub) => (
            <div
              key={sub.id}
              className={`rounded-xl shadow-sm border p-6 transition-all ${
                sub.isActive
                  ? 'border-gray-200 bg-white'
                  : 'border-gray-300 bg-gray-50 opacity-75'
              }`}
            >
              {editingId === sub.id ? (
                <div className="space-y-4">
                  <Input
                    label="Name"
                    value={formData.name || ''}
                    onChange={e => setFormData({ ...formData, name: e.target.value })}
                  />
                  <Textarea
                    label="Description"
                    value={formData.description || ''}
                    onChange={e => setFormData({ ...formData, description: e.target.value })}
                    rows={3}
                  />
                  <Input
                    label="Price"
                    value={formData.price || ''}
                    onChange={e => setFormData({ ...formData, price: e.target.value })}
                  />
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">What Includes</label>
                    {formData.whatIncludes?.map((item, i) => (
                      <div key={i} className="flex gap-2 mb-2">
                        <Input
                          value={item}
                          onChange={e => {
                            const updated = [...formData.whatIncludes];
                            updated[i] = e.target.value;
                            setFormData({ ...formData, whatIncludes: updated });
                          }}
                        />
                        <ActionButton size="sm" variant="outline" onClick={() => {
                          setFormData({ ...formData, whatIncludes: formData.whatIncludes.filter((_, idx) => idx !== i) });
                        }}>
                          Remove
                        </ActionButton>
                      </div>
                    ))}
                    <ActionButton size="sm" variant="outline" onClick={() => {
                      setFormData({ ...formData, whatIncludes: [...formData.whatIncludes, ''] });
                    }}>
                      + Add Include
                    </ActionButton>
                  </div>
                  <div className="flex items-center gap-4">
                    <label className="flex items-center gap-2">
                      <input
                        type="checkbox"
                        checked={formData.isActive}
                        onChange={e => setFormData({ ...formData, isActive: e.target.checked })}
                        className="rounded"
                      />
                      Active
                    </label>
                    <div className="flex gap-2">
                      <ActionButton onClick={handleSave}>Save</ActionButton>
                      <ActionButton variant="outline" onClick={handleCancel}>Cancel</ActionButton>
                    </div>
                  </div>
                </div>
              ) : (
                <div className="flex items-start justify-between">
                  <div className="flex-1">
                    <div className="flex items-center gap-3 mb-2">
                      <div className={`w-3 h-3 rounded-full ${sub.isActive ? 'bg-green-500' : 'bg-gray-400'}`} />
                      <h3 className="text-lg font-semibold text-gray-900">
                        {sub.name}
                        {!sub.isActive && <span className="text-sm text-gray-500 ml-2">(Inactive)</span>}
                      </h3>
                    </div>
                    <p className="text-sm text-gray-600 mb-3">{sub.description}</p>
                    <div className="text-2xl font-bold text-indigo-600 mb-3">{sub.price}</div>
                    <ul className="space-y-1 text-sm text-gray-700">
                      {sub.whatIncludes?.map((item, i) => (
                        <li key={i} className="flex items-center gap-2">
                          <span className="w-1.5 h-1.5 bg-green-500 rounded-full" />
                          {item}
                        </li>
                      ))}
                    </ul>
                  </div>
                  <div className="flex flex-col gap-2 ml-6">
                    <ActionButton size="sm" onClick={() => handleEdit(sub)}>Edit</ActionButton>
                    <ActionButton size="sm" variant="outline" onClick={() => handleDelete(sub.id)}>
                      Delete
                    </ActionButton>
                  </div>
                </div>
              )}
            </div>
          ))
        )}
      </div>

      {/* Пагинация */}
      {totalPages > 1 && (
        <div className="flex justify-center gap-2 mt-8">
          {[...Array(totalPages)].map((_, i) => (
            <button
              key={i}
              onClick={() => setCurrentPage(i + 1)}
              className={`w-10 h-10 rounded-full font-medium transition ${
                currentPage === i + 1
                  ? 'bg-indigo-600 text-white'
                  : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
              }`}
            >
              {i + 1}
            </button>
          ))}
        </div>
      )}

      {/* Аналитика */}
      <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-8">
        <h2 className="text-xl font-bold text-gray-900 mb-6">Subscription Analytics</h2>
        <div className="grid grid-cols-1 sm:grid-cols-3 gap-6">
          <div className="bg-blue-50 rounded-xl p-6 text-center">
            <p className="text-3xl font-bold text-blue-700">{subscriptions.filter(s => s.isActive).length * 3 || 0}</p>
            <p className="text-sm text-blue-600 mt-1">Total Subscribers</p>
          </div>
          <div className="bg-green-50 rounded-xl p-6 text-center">
            <p className="text-3xl font-bold text-green-700">${monthlyRevenue.toFixed(0)}</p>
            <p className="text-sm text-green-600 mt-1">Monthly Recurring</p>
          </div>
          <div className="bg-orange-50 rounded-xl p-6 text-center">
            <p className="text-3xl font-bold text-orange-700">${annualRevenue.toFixed(0)}</p>
            <p className="text-sm text-orange-600 mt-1">Annual Revenue</p>
          </div>
        </div>
      </div>

      {/* Модалка создания */}
      {showCreateModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
          <div className="bg-white rounded-xl shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto p-6">
            <h2 className="text-xl font-bold text-gray-900 mb-6">Create New Subscription</h2>
            <div className="space-y-4">
              <Input label="Name" value={createForm.name} onChange={e => setCreateForm({ ...createForm, name: e.target.value })} placeholder="e.g. Website Maintenance" />
              <Textarea label="Description" value={createForm.description} onChange={e => setCreateForm({ ...createForm, description: e.target.value })} rows={3} />
              <Input label="Price" value={createForm.price} onChange={e => setCreateForm({ ...createForm, price: e.target.value })} placeholder="$299/month" />
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">What Includes</label>
                {createForm.whatIncludes.map((item, i) => (
                  <div key={i} className="flex gap-2 mb-2">
                    <Input value={item} onChange={e => updateInclude(i, e.target.value)} placeholder="e.g. Up to 10 hours/month" />
                    {createForm.whatIncludes.length > 1 && (
                      <ActionButton size="sm" variant="outline" onClick={() => removeInclude(i)}>Remove</ActionButton>
                    )}
                  </div>
                ))}
                <ActionButton size="sm" variant="outline" onClick={addInclude}>+ Add Include</ActionButton>
              </div>
              <label className="flex items-center gap-2">
                <input type="checkbox" checked={createForm.isActive} onChange={e => setCreateForm({ ...createForm, isActive: e.target.checked })} className="rounded" />
                Active
              </label>
            </div>
            <div className="flex justify-end gap-3 mt-6">
              <ActionButton variant="outline" onClick={() => setShowCreateModal(false)}>Cancel</ActionButton>
              <ActionButton onClick={handleCreateSave}>Create Subscription</ActionButton>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}