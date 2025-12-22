import React from 'react';

const AuthLayout = ({ children }) => (
  <div className="flex min-h-screen bg-white">
    {/* === ФОРМА: 60% ширины на lg+ === */}
    <div className="w-full lg:w-3/5 xl:w-[50%] flex items-center justify-center p-6 sm:p-8 lg:p-12">
      <div className="w-full max-w-md">
        {children}
      </div>
    </div>

    {/* === КАРТИНКА: 40% ширины на lg+ === */}
    <div className="hidden lg:block lg:w-2/5 xl:w-[50%] relative overflow-hidden">
      <div
        className="absolute inset-0 bg-cover bg-center bg-no-repeat"
        style={{
          backgroundImage: `url('https://64.media.tumblr.com/781403bcb4c40ce6840f5d0659ffbad5/tumblr_okdnodmOCv1u7dwcso1_r1_1280.jpg')`,
        }}
      />
      <div className="absolute inset-0 " />
    </div>
  </div>
);

export default AuthLayout;

// src/layouts/AuthLayout.tsx

// import React from 'react';

// const AuthLayout = ({ children }) => (
//   <div className="flex min-h-screen w-full">

//     {/* Левая часть — форма */}
//     <div className="w-full lg:w-1/2 flex items-center justify-center p-4 sm:p-8 bg-white">
//       <div className="w-full max-w-md">
//         {children}
//       </div>
//     </div>

//     {/* Правая часть — картинка (только на десктопе) */}
//     <div className="hidden lg:block lg:w-1/2 relative overflow-hidden">
//       <div
//         className="absolute inset-0 bg-cover bg-center"
//         style={{
//           backgroundImage: `url('https://images.unsplash.com/photo-1518709268805-4e9042af9f23?w=1200&q=80')`,
//           filter: 'brightness(0.75)',
//         }}
//       />
//       <div className="absolute inset-0 bg-gradient-to-br from-blue-900/50 to-indigo-900/50" />
//     </div>

//     {/* Фон на мобильных (под формой) */}
//     <div
//       className="lg:hidden fixed inset-0 -z-10 bg-cover bg-center"
//       style={{
//         backgroundImage: `url('https://images.unsplash.com/photo-1518709268805-4e9042af9f23?w=1200&q=80')`,
//         filter: 'brightness(0.7)',
//       }}
//     />
//   </div>
// );

// export default AuthLayout;