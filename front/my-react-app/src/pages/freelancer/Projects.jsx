// src/pages/freelancer/Projects.jsx
import { useState, useEffect, useRef } from 'react';
import { useLocation } from 'react-router-dom';
import { ActionButton, Input, Select } from '../../components/ui';

const mockProjects = [
  { id: 1, title: 'SaaS Dashboard Design', client: 'CloudTech', status: 'Under Review', applied: false, daysAgo: '3 days ago', budget: '$2,500', category: 'Design' },
  { id: 2, title: 'Payment Integration', client: 'PaymentCo', status: 'Interview Scheduled', applied: true, daysAgo: '1 week ago', budget: '$1,800', category: 'Backend' },
  { id: 3, title: 'Mobile Banking App', client: 'FinBank', status: 'Open', applied: true, daysAgo: '2 days ago', budget: '$5,000', category: 'Mobile' },
  { id: 4, title: 'E-commerce Platform', client: 'ShopFast', status: 'Open', applied: false, daysAgo: '4 days ago', budget: '$3,200', category: 'Web Development' },
];

const categoryOptions = [
  { value: 'all', label: 'All' },
  { value: 'web', label: 'Web Development' },
  { value: 'mobile', label: 'Mobile' },
  { value: 'design', label: 'Design' },
  { value: 'backend', label: 'Backend' },
  { value: 'ai', label: 'AI/ML' },
];

const sortOptions = [
  { value: 'best', label: 'Best Match' },
  { value: 'recent', label: 'Most Recent' },
  { value: 'budget', label: 'Highest Budget' },
];

export default function FreelancerProjects() {
  const location = useLocation();
  const inputRef = useRef(null);

  const [showApplied, setShowApplied] = useState(false);
  const [search, setSearch] = useState('');
  const [category, setCategory] = useState('all');
  const [sort, setSort] = useState('best');

  // Подтягиваем search из URL при загрузке
  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const query = params.get('search');
    if (query) {
      setSearch(query);
      // Фокус на поле
      setTimeout(() => inputRef.current?.focus(), 100);
    }
  }, [location]);

  // Фильтрация и сортировка
  let filtered = mockProjects;

  if (showApplied) filtered = filtered.filter(p => p.applied);
  if (search) {
    filtered = filtered.filter(p =>
      p.title.toLowerCase().includes(search.toLowerCase()) ||
      p.client.toLowerCase().includes(search.toLowerCase())
    );
  }
  if (category !== 'all') {
    filtered = filtered.filter(p => p.category.toLowerCase().includes(category));
  }
  if (sort === 'recent') {
    filtered = [...filtered].sort((a, b) => a.daysAgo.localeCompare(b.daysAgo));
  } else if (sort === 'budget') {
    filtered = [...filtered].sort((a, b) => parseInt(b.budget.replace(/[^0-9]/g, '')) - parseInt(a.budget.replace(/[^0-9]/g, '')));
  }

  return (
    <div className="max-w-5xl mx-auto space-y-8 pb-12">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Projects</h1>
          <p className="text-sm text-gray-600">Find and apply for projects that match your skills</p>
        </div>
        <div className="flex items-center gap-3">
          <ActionButton
            size="sm"
            variant={showApplied ? 'primary' : 'outline'}
            onClick={() => setShowApplied(!showApplied)}
          >
            Applied ({mockProjects.filter(p => p.applied).length})
          </ActionButton>
          <ActionButton size="sm">Browse Projects</ActionButton>
        </div>
      </div>

      <div className="flex flex-col sm:flex-row gap-3">
        <Input
          ref={inputRef}
          placeholder="Search projects..."
          value={search}
          onChange={e => setSearch(e.target.value)}
          className="flex-1"
        />
        <Select value={category} onChange={e => setCategory(e.target.value)} options={categoryOptions} className="w-full sm:w-48" />
        <Select value={sort} onChange={e => setSort(e.target.value)} options={sortOptions} className="w-full sm:w-48" />
      </div>

      <div className="space-y-6">
        {filtered.map(project => (
          <div key={project.id} className="bg-white rounded-xl shadow-sm border border-gray-200 p-6 hover:shadow-md transition">
            <div className="flex items-start justify-between">
              <div className="flex-1">
                <h3 className="text-lg font-semibold text-gray-900">{project.title}</h3>
                <div className="flex items-center gap-3 mt-1 text-sm text-gray-600">
                  <span>{project.client}</span>
                  <span>•</span>
                  <span className="px-2 py-0.5 bg-gray-100 rounded-full text-xs">{project.status}</span>
                  {project.applied && <span className="px-2 py-0.5 bg-indigo-100 text-indigo-700 rounded-full text-xs">Applied</span>}
                </div>
                <div className="flex items-center gap-4 mt-2 text-sm text-gray-500">
                  <span>{project.daysAgo}</span>
                  <span>•</span>
                  <span className="font-medium text-gray-700">Budget: {project.budget}</span>
                </div>
              </div>
              <ActionButton size="sm" variant="outline">View Details</ActionButton>
            </div>
          </div>
        ))}

        {filtered.length === 0 && (
          <p className="text-center text-gray-500 py-12">No projects found</p>
        )}
      </div>
    </div>
  );
}