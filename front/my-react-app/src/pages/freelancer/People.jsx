// src/pages/freelancer/People.jsx
import { useState, useEffect, useRef } from 'react';
import { useLocation } from 'react-router-dom';
import { ActionButton, Input, Select } from '../../components/ui';

const mockPeople = [
  { id: 1, name: 'Anna Petrova', role: 'Freelancer', rating: 4.9, skills: 'React, TypeScript', verified: true },
  { id: 2, name: 'Dmitry Sokolov', role: 'Employer', rating: 5.0, skills: 'Product Manager', verified: true },
  { id: 3, name: 'Maria Ivanova', role: 'Freelancer', rating: 4.8, skills: 'UI/UX Design, Figma', verified: false },
  { id: 4, name: 'Alexey Kuznetsov', role: 'Employer', rating: 4.7, skills: 'Startup Founder', verified: true },
  { id: 5, name: 'Elena Morozova', role: 'Freelancer', rating: 5.0, skills: 'Node.js, Python', verified: true },
  { id: 6, name: 'Sergey Volkov', role: 'Employer', rating: 4.9, skills: 'CTO', verified: true },
];

const roleOptions = [
  { value: 'all', label: 'All People' },
  { value: 'freelancer', label: 'Freelancers' },
  { value: 'employer', label: 'Employers' },
];

export default function FreelancerPeople() {
  const location = useLocation();
  const inputRef = useRef(null);

  const [search, setSearch] = useState('');
  const [roleFilter, setRoleFilter] = useState('all');

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const query = params.get('search');
    if (query) {
      setSearch(query);
      setTimeout(() => inputRef.current?.focus(), 100);
    }
  }, [location]);

  let filtered = mockPeople;

  if (search) {
    filtered = filtered.filter(p =>
      p.name.toLowerCase().includes(search.toLowerCase()) ||
      p.skills.toLowerCase().includes(search.toLowerCase())
    );
  }

  if (roleFilter !== 'all') {
    filtered = filtered.filter(p => p.role.toLowerCase() === roleFilter);
  }

  return (
    <div className="max-w-5xl mx-auto space-y-8 pb-12">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">People</h1>
          <p className="text-sm text-gray-600">Find freelancers and employers</p>
        </div>
      </div>

      <div className="flex flex-col sm:flex-row gap-3">
        <Input
          ref={inputRef}
          placeholder="Search people by name or skills..."
          value={search}
          onChange={e => setSearch(e.target.value)}
          className="flex-1"
        />
        <Select
          value={roleFilter}
          onChange={e => setRoleFilter(e.target.value)}
          options={roleOptions}
          className="w-full sm:w-64"
        />
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {filtered.map(person => (
          <div
            key={person.id}
            className="bg-white rounded-xl shadow-sm border border-gray-200 p-6 hover:shadow-md transition cursor-pointer"
          >
            <div className="flex items-start justify-between mb-4">
              <div className="w-16 h-16 bg-gradient-to-br from-indigo-400 to-purple-500 rounded-full flex items-center justify-center text-white text-xl font-bold">
                {person.name.split(' ').map(n => n[0]).join('')}
              </div>
              {person.verified && (
                <span className="px-2 py-1 bg-blue-100 text-blue-700 text-xs rounded-full flex items-center gap-1">
                  Verified
                </span>
              )}
            </div>

            <h3 className="text-lg font-semibold text-gray-900">{person.name}</h3>
            <p className="text-sm text-indigo-600 font-medium">{person.role}</p>
            <div className="flex items-center gap-1 mt-2">
              <span className="text-yellow-500">★</span>
              <span className="text-sm font-medium text-gray-700">{person.rating}</span>
            </div>
            <p className="text-sm text-gray-600 mt-3 line-clamp-2">{person.skills}</p>

            <div className="mt-4 pt-4 border-t border-gray-100 flex gap-2">
              <ActionButton size="sm" variant="outline" className="flex-1">
                View Profile
              </ActionButton>
              <ActionButton size="sm" className="flex-1">
                Message
              </ActionButton>
            </div>
          </div>
        ))}
      </div>

      {filtered.length === 0 && (
        <p className="text-center text-gray-500 py-12">No people found</p>
      )}
    </div>
  );
}