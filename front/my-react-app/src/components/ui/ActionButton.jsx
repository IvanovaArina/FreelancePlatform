// src/components/ui/ActionButton.jsx
import React from 'react';

export const ActionButton = ({ children, onClick, variant = 'primary', size = 'md' }) => {
  const baseClasses = 'font-medium rounded-lg transition focus:outline-none focus:ring-2 focus:ring-offset-2';
  const sizeClasses = size === 'sm' ? 'px-4 py-1.5 text-sm' : 'px-6 py-2.5 text-base';
  
  const variantClasses = {
    primary: 'bg-indigo-600 text-white hover:bg-indigo-700 focus:ring-indigo-500',
    outline: 'border border-indigo-600 text-indigo-600 hover:bg-indigo-50 focus:ring-indigo-500',
  };

  return (
    <button
      onClick={onClick}
      className={`${baseClasses} ${sizeClasses} ${variantClasses[variant]} inline-flex items-center justify-center`}
    >
      {children}
    </button>
  );
};