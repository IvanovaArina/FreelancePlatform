import React from 'react';

export const Input = ({ label, type = 'text', placeholder, value, onChange, name, error }) => (
  <div className="mb-4">
    <label className="block text-sm font-medium text-gray-700 mb-2">
      {label}
    </label>
    <input
      type={type}
      placeholder={placeholder}
      value={value}
      onChange={onChange}
      name={name}
      className={`w-full px-4 py-3 border ${error ? 'border-red-500' : 'border-gray-300'} rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition`}
    />
    {error && <p className="mt-1 text-sm text-red-500">{error}</p>}
  </div>
);