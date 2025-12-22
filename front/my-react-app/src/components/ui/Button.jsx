import React from 'react';

export const Button = ({ children, variant = 'primary', onClick, type = 'button', className = '' }) => {
  const baseClasses = "w-full py-3 rounded-lg font-medium transition duration-200";
  const variants = {
    primary: "bg-indigo-600 text-white hover:bg-indigo-700",
    outline: "bg-white border border-gray-300 text-gray-700 hover:bg-gray-50"
  };
  
  return (
    <button 
      className={`${baseClasses} ${variants[variant]} ${className}`}
      onClick={onClick}
      type={type}
    >
      {children}
    </button>
  );
};