// src/components/ui/Textarea.jsx
import React from 'react';

export const Textarea = ({ label, placeholder, value, onChange, name, error, rows = 3, className = '' }) => (
  <div className="mb-4">
    {label && (
      <label className="block text-sm font-medium text-gray-700 mb-2">
        {label}
      </label>
    )}
    <textarea
      placeholder={placeholder}
      value={value}
      onChange={onChange}
      name={name}
      rows={rows}
      className={`w-full px-4 py-3 border ${error ? 'border-red-500' : 'border-gray-300'} rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition resize-none ${className}`}
    />
    {error && <p className="mt-1 text-sm text-red-500">{error}</p>}
  </div>
);