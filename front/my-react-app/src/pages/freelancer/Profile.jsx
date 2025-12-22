// src/pages/freelancer/Profile.jsx
import { useState, useEffect } from 'react';
import { useAuth } from '../../hooks/useAuth';
import { Input, Textarea, ActionButton } from '../../components/ui';
import { useNavigate } from 'react-router-dom';

import { meApi } from '../../api/meApi';
import { freelancerProfileApi } from '../../api/freelancerProfileApi';
import { portfolioApi } from '../../api/portfolioApi'; // ← твой http.js

const mockReviews = [
  { id: 1, author: 'Client A', rating: 5, text: 'Amazing work, delivered on time!', date: '2025-02-15' },
  { id: 2, author: 'Client B', rating: 4, text: 'Great communication, solid code.', date: '2025-02-10' },
  { id: 3, author: 'Client C', rating: 5, text: 'Will hire again!', date: '2025-01-28' },
  { id: 4, author: 'Client D', rating: 5, text: 'Top-tier developer.', date: '2025-01-20' },
  { id: 5, author: 'Client E', rating: 4, text: 'Good job overall.', date: '2025-01-15' },
  { id: 6, author: 'Client F', rating: 5, text: 'Exceeded expectations!', date: '2025-01-05' },
];

export default function FreelancerProfile() {
  const navigate = useNavigate();
  const { user } = useAuth();

  // Режимы
  const [isEditingName, setIsEditingName] = useState(false);
  const [isEditingProfile, setIsEditingProfile] = useState(false);
  const [showAvatarMenu, setShowAvatarMenu] = useState(false);
  const [showAddSkill, setShowAddSkill] = useState(false);
  const [newSkill, setNewSkill] = useState('');
  const [selectedFile, setSelectedFile] = useState(null);
  
  // Пагинация
  const [portfolioPage, setPortfolioPage] = useState(1);
  const [reviewsPage, setReviewsPage] = useState(1);
  const itemsPerPage = 4;

  // Профиль
  const [formData, setFormData] = useState({
    fullName: '',
    jobTitle: '',
    jobTitleEdit: '',
    bio: '',
    bioEdit: '',
    skills: [],
    verified: false,
  });

  // Портфолио
  const [portfolio, setPortfolio] = useState([]);
  const [showAddProject, setShowAddProject] = useState(false);
  const [editingProject, setEditingProject] = useState(null);

  const [projectForm, setProjectForm] = useState({
    title: '',
    description: '',
    imageUrl: '',
  });

  // === Загрузка данных ===
  useEffect(() => {
    // Имя
    meApi.getName()
      .then(res => setFormData(prev => ({ ...prev, fullName: res.data })))
      .catch(() => {});

    // Профиль фрилансера
    freelancerProfileApi.getMyProfile()
      .then(res => {
        const p = res.data;
        setFormData(prev => ({
          ...prev,
          jobTitle: p.jobTitle || '',
          jobTitleEdit: p.jobTitle || '',
          bio: p.bio || '',
          bioEdit: p.bio || '',
          skills: p.skills || [],
          verified: p.verified || false,
        }));
      })
      .catch(() => {});

    // Портфолио с бэка
    loadPortfolio();
  }, []);

  const loadPortfolio = () => {
    portfolioApi.getMyPortfolio()
      .then(res => setPortfolio(res.data))
      .catch(err => console.error('Ошибка загрузки портфолио:', err));
  };

  // === Инициалы ===
  const initials = formData.fullName
    .split(' ')
    .map(n => n[0])
    .join('')
    .slice(0, 2)
    .toUpperCase();

  // === Сохранение профиля ===
  const handleSaveProfile = async () => {
    try {
      await freelancerProfileApi.updateMyProfile({
        jobTitle: formData.jobTitleEdit,
        bio: formData.bioEdit,
        skills: formData.skills,
        hourlyRate: 0,
        profileImageUrl: null,
      });
      setFormData(prev => ({
        ...prev,
        jobTitle: prev.jobTitleEdit,
        bio: prev.bioEdit,
      }));
      setIsEditingProfile(false);
      alert('Profile updated!');
    } catch {
      alert('Failed to update profile');
    }
  };

  const handleSaveName = async () => {
    try {
      await meApi.changeName(formData.fullName);
      setIsEditingName(false);
      alert('Name changed!');
    } catch {
      alert('Failed to change name');
    }
  };

  const addSkill = async () => {
    if (!newSkill.trim()) return;
    const updated = [...formData.skills, newSkill.trim()];
    try {
      await freelancerProfileApi.updateMyProfile({
        jobTitle: formData.jobTitle,
        bio: formData.bio,
        skills: updated,
        hourlyRate: 0,
        profileImageUrl: null,
      });
      setFormData(prev => ({ ...prev, skills: updated }));
      setNewSkill('');
      setShowAddSkill(false);
    } catch {}
  };

  const removeSkill = async (skill) => {
    const updated = formData.skills.filter(s => s !== skill);
    try {
      await freelancerProfileApi.updateMyProfile({
        jobTitle: formData.jobTitle,
        bio: formData.bio,
        skills: updated,
        hourlyRate: 0,
        profileImageUrl: null,
      });
      setFormData(prev => ({ ...prev, skills: updated }));
    } catch {}
  };

  // === Портфолио ===
  const handleSaveProject = async () => {
  try {
    let tempImageKey = null;

    // 1) Если выбрали файл → загружаем во временное хранилище ImageService
    if (selectedFile) {
      const formData = new FormData();
      formData.append("file", selectedFile);
        // Вместо http://localhost:7002/api/images/upload-temp
        const res = await fetch("http://localhost:5270/api/images/upload-temp", {
        method: "POST",
        body: formData
      });

      tempImageKey = (await res.json()).tempImageKey;
    }

    let saved;
    if (editingProject) {
      await portfolioApi.updateProject(editingProject.id, {
        title: projectForm.title,
        description: projectForm.description,
        tempImageKey
      });
      saved = { ...projectForm, id: editingProject.id };
    } else {
      const res = await portfolioApi.createProject({
        title: projectForm.title,
        description: projectForm.description,
        tempImageKey
      });
      saved = res.data;
    }

    loadPortfolio();

    setProjectForm({ title: '', description: '', imageUrl: '' });
    setSelectedFile(null);
    setEditingProject(null);
    setShowAddProject(false);
    alert('Project saved!');
  } catch (err) {
    console.error(err);
    alert('Error saving project');
  }
};

  const handleDeleteProject = async (id) => {
    if (!confirm('Delete this project?')) return;
    try {
      await portfolioApi.deleteProject(id);
      setPortfolio(prev => prev.filter(p => p.id !== id));
      alert('Project deleted');
    } catch {
      alert('Error deleting project');
    }
  };

  const openEditProject = (project) => {
    setEditingProject(project);
    setProjectForm({
      title: project.title,
      description: project.description,
      imageUrl: project.imageUrl || '',
    });
    setShowAddProject(true);
  };

  // Пагинация портфолио
  const portfolioItems = portfolio.slice((portfolioPage - 1) * itemsPerPage, portfolioPage * itemsPerPage);
  const totalPortfolioPages = Math.ceil(portfolio.length / itemsPerPage);

  // Пагинация отзывов
  const reviewItems = mockReviews.slice((reviewsPage - 1) * 5, reviewsPage * 5);
  const totalReviewPages = Math.ceil(mockReviews.length / 5);

  return (
    <div className="max-w-5xl mx-auto space-y-8 pb-12">

      {/* БЛОК 1: Основная информация */}
      <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-8">
        <div className="flex items-start justify-between">
          <div className="flex items-center space-x-6">
            <div className="relative">
              <button
                onClick={() => setShowAvatarMenu(!showAvatarMenu)}
                className="w-24 h-24 bg-gradient-to-br from-indigo-400 to-purple-500 rounded-full flex items-center justify-center text-white text-2xl font-bold shadow-lg hover:ring-4 hover:ring-indigo-200 transition"
              >
                {initials}
              </button>
              {showAvatarMenu && (
                <div className="absolute top-full left-0 mt-2 w-48 bg-white rounded-lg shadow-xl border border-gray-200 z-10">
                  <button className="w-full text-left px-4 py-2 text-sm hover:bg-gray-50">View Photo</button>
                  <button className="w-full text-left px-4 py-2 text-sm hover:bg-gray-50 text-indigo-600 font-medium">Change Photo</button>
                </div>
              )}
            </div>

            <div className="flex-1">
              <div className="flex items-center gap-4 mb-2">
                <div className="flex items-center gap-3">
                  <h1 className="text-2xl font-bold text-gray-900">
                    {isEditingName ? (
                      <Input
                        value={formData.fullName}
                        onChange={e => setFormData(prev => ({ ...prev, fullName: e.target.value }))}
                        className="text-2xl font-bold min-w-80"
                        autoFocus
                      />
                    ) : (
                      formData.fullName || 'Your Name'
                    )}
                  </h1>
                  {!isEditingName && !isEditingProfile && (
                    <button onClick={() => setIsEditingName(true)} className="text-sm font-medium text-indigo-600 hover:text-indigo-700 underline">
                      Change Name
                    </button>
                  )}
                </div>
                {isEditingName && (
                  <div className="flex gap-2">
                    <ActionButton size="sm" onClick={handleSaveName}>Save</ActionButton>
                    <ActionButton size="sm" variant="outline" onClick={() => setIsEditingName(false)}>Cancel</ActionButton>
                  </div>
                )}
              </div>

              {!isEditingName && !isEditingProfile && !formData.verified && (
                <ActionButton size="sm" variant="outline" onClick={() => navigate('/freelancer/verification')}>
                  Verify
                </ActionButton>
              )}
              {formData.verified && (
                <span className="px-3 py-1 bg-blue-100 text-blue-700 rounded-full text-sm font-medium">
                  Verified
                </span>
              )}

              {isEditingProfile ? (
                <Input
                  value={formData.jobTitleEdit}
                  onChange={e => setFormData(prev => ({ ...prev, jobTitleEdit: e.target.value }))}
                  className="mt-3 text-lg font-medium text-indigo-600 w-full max-w-2xl"
                />
              ) : (
                <p className="mt-3 text-lg font-medium text-indigo-600">{formData.jobTitle || 'Your Title'}</p>
              )}

              <div className="flex items-center gap-4 mt-3 text-sm text-gray-600">
                <span className="flex items-center gap-1.5">
                  <span className="text-yellow-400">★</span>
                  <span className="font-medium">4.9</span>
                  <span>(48 reviews)</span>
                </span>
                <span>•</span>
                <span>47 completed projects</span>
                <span>•</span>
                <span className="px-2 py-0.5 bg-indigo-100 text-indigo-700 rounded-full text-xs font-medium">Top Rated</span>
              </div>

              {isEditingProfile ? (
                <Textarea
                  value={formData.bioEdit}
                  onChange={e => setFormData(prev => ({ ...prev, bioEdit: e.target.value }))}
                  className="mt-5 w-full max-w-3xl"
                  rows={4}
                />
              ) : (
                <p className="mt-5 text-gray-700 leading-relaxed max-w-3xl">{formData.bio || 'No bio yet'}</p>
              )}
            </div>
          </div>

          {!isEditingName && (
            <ActionButton
              size="lg"
              onClick={() => {
                if (isEditingProfile) handleSaveProfile();
                else {
                  setFormData(prev => ({
                    ...prev,
                    jobTitleEdit: prev.jobTitle,
                    bioEdit: prev.bio,
                  }));
                  setIsEditingProfile(true);
                }
              }}
            >
              {isEditingProfile ? 'Save Changes' : 'Edit Profile'}
            </ActionButton>
          )}
        </div>

        {isEditingProfile && !isEditingName && (
          <div className="mt-8 flex justify-end">
            <ActionButton variant="outline" onClick={() => setIsEditingProfile(false)}>Cancel</ActionButton>
          </div>
        )}
      </div>

      {/* БЛОК 2: Skills */}
      <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-8">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-xl font-bold text-gray-900">Skills</h2>
          {isEditingProfile && !showAddSkill && (
            <ActionButton size="sm" onClick={() => setShowAddSkill(true)}>Add Skill</ActionButton>
          )}
          {isEditingProfile && showAddSkill && (
            <div className="flex items-center gap-2">
              <Input value={newSkill} onChange={e => setNewSkill(e.target.value)} onKeyDown={e => e.key === 'Enter' && addSkill()} placeholder="e.g. Docker, AWS" className="w-64" autoFocus />
              <ActionButton size="sm" onClick={addSkill}>Add</ActionButton>
              <ActionButton size="sm" variant="outline" onClick={() => { setShowAddSkill(false); setNewSkill(''); }}>Cancel</ActionButton>
            </div>
          )}
        </div>
        <div className="flex flex-wrap gap-3">
          {formData.skills.map(skill => (
            <span key={skill} className="inline-flex items-center gap-2 px-4 py-2 bg-indigo-50 text-indigo-700 rounded-full text-sm font-medium hover:bg-indigo-100 transition">
              {skill}
              {isEditingProfile && (
                <button onClick={() => removeSkill(skill)} className="text-indigo-600 hover:text-red-600 font-bold">×</button>
              )}
            </span>
          ))}
        </div>
      </div>

      {/* БЛОК 3: Portfolio — С БЭКЕНДОМ + ПАГИНАЦИЯ */}
<div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-8">
  <div className="flex items-center justify-between mb-6">
    <h2 className="text-xl font-bold text-gray-900">Portfolio</h2>
    <ActionButton size="sm" onClick={() => setShowAddProject(true)}>Add Project</ActionButton>
  </div>

  <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
    {portfolioItems.length === 0 ? (
      <p className="col-span-2 text-center text-gray-500 py-12">
        No projects yet. Add your first one!
      </p>
    ) : (
      portfolioItems.map(project => (
        <div key={project.id}
          className="group relative rounded-xl overflow-hidden shadow-md hover:shadow-xl transition bg-white">
          
          <img
            src={project.imageUrl || 'https://via.placeholder.com/300x200'}
            alt={project.title}
            className="w-full h-52 object-cover group-hover:scale-105 transition"
          />

          <div className="p-4">
            <h3 className="font-semibold text-gray-900">{project.title}</h3>
            <p className="text-sm text-gray-600 mt-1">{project.description}</p>

            {/* Статус загрузки */}
            {project.imageStatus === "pending" && (
              <p className="mt-2 text-sm text-yellow-600 font-medium">
                Processing image...
              </p>
            )}
          </div>

          {/* Кнопки Edit / Delete */}
          <div className="absolute top-2 right-2 opacity-0 group-hover:opacity-100 transition flex flex-col gap-2">
            <button onClick={() => openEditProject(project)}
              className="bg-white p-2 rounded-full shadow-lg hover:bg-gray-100 text-sm font-medium">
              Edit
            </button>
            <button onClick={() => handleDeleteProject(project.id)}
              className="bg-red-500 text-white p-2 rounded-full shadow-lg hover:bg-red-600 text-sm font-medium">
              Delete
            </button>
          </div>
        </div>
      ))
    )}
  </div>


        {/* Пагинация портфолио */}
        {totalPortfolioPages > 1 && (
    <div className="mt-8 flex justify-center gap-2">
      {[...Array(totalPortfolioPages)].map((_, i) => (
        <button
          key={i}
          onClick={() => setPortfolioPage(i + 1)}
          className={`w-10 h-10 rounded-full font-medium transition ${
            portfolioPage === i + 1
              ? 'bg-indigo-600 text-white'
              : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
          }`}
        >
          {i + 1}
        </button>
      ))}
    </div>
  )}
</div>

      {/* БЛОК 4: Reviews */}
      <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-8">
        <h2 className="text-xl font-bold text-gray-900 mb-6">Client Reviews</h2>
        <div className="space-y-8">
          {reviewItems.map(review => (
            <div key={review.id} className="border-b border-gray-100 pb-8 last:border-0 last:pb-0">
              <div className="flex items-start justify-between">
                <div>
                  <div className="flex items-center gap-3 mb-2">
                    <span className="font-semibold text-gray-900">{review.author}</span>
                    <div className="flex gap-0.5">
                      {[...Array(5)].map((_, i) => (
                        <span key={i} className={i < review.rating ? 'text-yellow-400' : 'text-gray-300'}>★</span>
                      ))}
                    </div>
                  </div>
                  <p className="text-gray-700 leading-relaxed">{review.text}</p>
                </div>
                <span className="text-sm text-gray-500">{review.date}</span>
              </div>
            </div>
          ))}
        </div>

        {totalReviewPages > 1 && (
          <div className="mt-8 flex justify-center gap-2">
            {[...Array(totalReviewPages)].map((_, i) => (
              <button
                key={i}
                onClick={() => setReviewsPage(i + 1)}
                className={`w-10 h-10 rounded-full font-medium transition ${
                  reviewsPage === i + 1
                    ? 'bg-indigo-600 text-white'
                    : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
                }`}
              >
                {i + 1}
              </button>
            ))}
          </div>
        )}
      </div>

      {/* Модалка Add / Edit Project */}
      {showAddProject && (
  <div
    className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
    onClick={() => {
      setShowAddProject(false);
      setEditingProject(null);
      setProjectForm({ title: '', description: '', imageUrl: '', tempImageKey: null });
      setSelectedFile(null);
    }}
  >
    <div
      className="bg-white rounded-2xl p-8 max-w-lg w-full"
      onClick={e => e.stopPropagation()}
    >
      <h3 className="text-2xl font-bold mb-6">
        {editingProject ? 'Edit Project' : 'Add New Project'}
      </h3>

      <Input
        placeholder="Project title"
        value={projectForm.title}
        onChange={e => setProjectForm(prev => ({ ...prev, title: e.target.value }))}
        className="mb-4"
      />

      <Textarea
        placeholder="Brief description..."
        rows={4}
        value={projectForm.description}
        onChange={e => setProjectForm(prev => ({ ...prev, description: e.target.value }))}
        className="mb-4"
      />

      {/* File input for image */}
      <input
        type="file"
        onChange={e => setSelectedFile(e.target.files[0])}
        className="mb-4"
      />

      <div className="flex justify-end gap-3">
        <ActionButton
          variant="outline"
          onClick={() => {
            setShowAddProject(false);
            setEditingProject(null);
            setProjectForm({ title: '', description: '', imageUrl: '' });
            setSelectedFile(null);
          }}
        >
          Cancel
        </ActionButton>

        <ActionButton onClick={handleSaveProject}>
          {editingProject ? 'Save Changes' : 'Add Project'}
        </ActionButton>
      </div>
    </div>
  </div>
)}
    </div>
  );
}