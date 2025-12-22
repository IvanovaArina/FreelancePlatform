// src/components/freelancer/AssistantWidget.jsx
import { ActionButton } from '../../components/ui';

export default function AssistantWidget({ onClose }) {
  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div
        className="bg-white rounded-2xl shadow-2xl max-w-4xl w-full max-h-[90vh] overflow-y-auto p-8 space-y-8"
        onClick={(e) => e.stopPropagation()}
      >
        {/* === Кнопка закрытия === */}
        <button
          onClick={onClose}
          className="absolute top-6 right-6 text-gray-400 hover:text-gray-600 transition"
        >
          <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>

        {/* === Hi, John! === */}
        <div className="text-center">
          <h2 className="text-2xl font-bold text-gray-900">Hi, John!</h2>
          <p className="text-base text-gray-600 mt-1">I have some new suggestions for you</p>
        </div>

        {/* === Suggestions === */}
        <div className="space-y-6">
          <div className="bg-gradient-to-r from-yellow-50 to-amber-50 rounded-xl p-6 border border-yellow-200">
            <div className="flex items-start justify-between gap-4">
              <div className="flex-1">
                <p className="text-lg font-semibold text-gray-900">Profile Completion</p>
                <p className="text-base text-gray-700 mt-2">
                  Complete your profile — <span className="text-red-600 font-bold">High Priority</span>
                </p>
                <p className="text-sm text-gray-600 mt-1">
                  Adding a portfolio can increase your visibility by <strong>60%</strong>
                </p>
              </div>
              <ActionButton size="md">Update Profile</ActionButton>
            </div>
          </div>

          <div className="bg-gradient-to-r from-blue-50 to-cyan-50 rounded-xl p-6 border border-blue-200">
            <div className="flex items-start justify-between gap-4">
              <div className="flex-1">
                <p className="text-lg font-semibold text-gray-900">Learn TypeScript</p>
                <p className="text-base text-gray-700 mt-2">
                  High demand skill in your area. <strong>15+ projects available</strong>.
                </p>
              </div>
              <ActionButton size="md" variant="outline">View Courses</ActionButton>
            </div>
          </div>

          <div className="bg-gradient-to-r from-purple-50 to-pink-50 rounded-xl p-6 border border-purple-200">
            <div className="flex items-start justify-between gap-4">
              <div className="flex-1">
                <p className="text-lg font-semibold text-gray-900">New Project Match</p>
                <p className="text-base text-gray-700 mt-2">
                  React Developer needed — <span className="text-red-600 font-bold">High Priority</span>
                </p>
                <p className="text-sm text-gray-600 mt-1">
                  Matches <strong>95%</strong> of your skills
                </p>
              </div>
              <ActionButton size="md" variant="outline">View Project</ActionButton>
            </div>
          </div>
        </div>

        {/* === Recommended Skills === */}
        <div>
          <h3 className="text-lg font-semibold text-gray-900 mb-5">Recommended Skills</h3>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {[
              { name: 'React', demand: 95, icon: 'R' },
              { name: 'TypeScript', demand: 88, icon: 'TS' },
              { name: 'Node.js', demand: 82, icon: 'N' },
              { name: 'Python', demand: 78, icon: 'P' },
              { name: 'AWS', demand: 75, icon: 'A' },
              { name: 'Docker', demand: 70, icon: 'D' },
            ].map(skill => (
              <div key={skill.name} className="bg-gray-50 rounded-xl p-4 border border-gray-200">
                <div className="flex items-center gap-4">
                  <div className="w-12 h-12 bg-gradient-to-br from-indigo-500 to-purple-600 rounded-lg flex items-center justify-center text-white font-bold text-sm">
                    {skill.icon}
                  </div>
                  <div className="flex-1">
                    <div className="flex items-center justify-between mb-1">
                      <span className="font-semibold text-gray-900">{skill.name}</span>
                      <span className="text-sm text-gray-500">{skill.demand}% demand</span>
                    </div>
                    <div className="w-full bg-gray-300 rounded-full h-3">
                      <div
                        className="bg-gradient-to-r from-indigo-500 to-purple-600 h-3 rounded-full transition-all duration-700"
                        style={{ width: `${skill.demand}%` }}
                      />
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* === Learning Resources === */}
        <div>
          <h3 className="text-lg font-semibold text-gray-900 mb-5">Learning Resources</h3>
          <div className="space-y-3">
            {[
              { title: 'TypeScript Best Practices', author: 'Frontend Masters', time: '4 hours', level: 'Intermediate' },
              { title: 'Advanced React Patterns', author: 'Udemy', time: '8 hours', level: 'Advanced' },
              { title: 'Node.js Microservices', author: 'Pluralsight', time: '6 hours', level: 'Advanced' },
              { title: 'AWS Certified Developer', author: 'A Cloud Guru', time: '12 hours', level: 'Professional' },
              { title: 'Docker & Kubernetes', author: 'Udemy', time: '10 hours', level: 'Intermediate' },
            ].map((course, i) => (
              <div
                key={i}
                className="flex items-center justify-between p-5 bg-gradient-to-r from-gray-50 to-gray-100 rounded-xl hover:from-gray-100 hover:to-gray-200 transition cursor-pointer border border-gray-200"
              >
                <div className="flex items-center gap-4">
                  <div className="w-14 h-14 bg-gradient-to-br from-indigo-100 to-purple-100 rounded-lg flex items-center justify-center">
                    <svg className="w-8 h-8 text-indigo-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
                    </svg>
                  </div>
                  <div>
                    <p className="text-base font-semibold text-gray-900">{course.title}</p>
                    <p className="text-sm text-gray-600">{course.author} • {course.time}</p>
                    <span className="inline-block mt-1 px-2 py-0.5 text-xs font-medium bg-indigo-100 text-indigo-700 rounded-full">
                      {course.level}
                    </span>
                  </div>
                </div>
                <svg className="w-6 h-6 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5l7 7-7 7" />
                </svg>
              </div>
            ))}
          </div>
        </div>

        {/* === CTA === */}
        <div className="text-center pt-4">
          <ActionButton size="lg">Start Improving Today</ActionButton>
        </div>
      </div>
    </div>
  );
}