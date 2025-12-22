import React from 'react';

export const Checkbox = ({ label, checked, onChange, name }) => (
  <div className="flex items-center mb-4">
    <input
      type="checkbox"
      checked={checked}
      onChange={onChange}
      name={name}
      className="w-4 h-4 text-indigo-600 border-gray-300 rounded focus:ring-indigo-500"
    />
    <label className="ml-2 text-sm text-gray-600">{label}</label>
  </div>
);