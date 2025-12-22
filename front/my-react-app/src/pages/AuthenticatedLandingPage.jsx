// src/pages/AuthenticatedLandingPage.jsx
import { Link } from 'react-router-dom';
import FreelancerHeader from '../components/layout/FreelancerHeader';
import Footer from '../components/layout/Footer';
import { Button } from '../components/ui/Button';

// Дублируем данные из LandingPage, чтобы не импортировать
const testimonials = [
  { name: 'Alice Johnson', role: 'Creative Designer', text: 'ConnectFreelance has transformed my freelance career. Highly recommend it!', rating: 5 },
  { name: 'Mark Lee', role: 'Web Developer', text: 'A fantastic platform for finding work and connecting with clients!', rating: 5 },
  { name: 'Sara Kim', role: 'Graphic Artist', text: 'The dashboard is user-friendly and insightful. Love it!', rating: 5 },
];

const categories = [
  { title: 'Web Dev', icon: '💻', color: 'from-indigo-500 to-blue-600' },
  { title: 'МMobile Dev', icon: '📱', color: 'from-purple-500 to-pink-600' },
  { title: 'Designe', icon: '🎨', color: 'from-pink-500 to-rose-600' },
  { title: 'Data / AI', icon: '🧠', color: 'from-emerald-500 to-teal-600' },
  { title: 'DevOps / QA', icon: '⚙️', color: 'from-orange-500 to-red-600' },
  { title: 'Etc.', icon: '✨', color: 'from-gray-500 to-slate-600' },
];

export default function AuthenticatedLandingPage() {
  return (
    <div className="min-h-screen bg-gradient-to-b from-indigo-50 via-white to-purple-50">
      <FreelancerHeader />

      {/* Hero */}
      <section className="pt-20 pb-32 px-4">
        <div className="max-w-7xl mx-auto text-center">
          <h1 className="text-5xl md:text-6xl font-bold bg-gradient-to-r from-indigo-600 via-purple-600 to-pink-600 bg-clip-text text-transparent">
            Welcome back to Connect!
          </h1>
          <p className="mt-6 text-xl text-gray-600 max-w-3xl mx-auto">
            Find new projects, grow your portfolio, and earn more — all in one place.
          </p>
          <div className="mt-10 flex flex-col sm:flex-row gap-4 justify-center">
            <Link to="/freelancer/projects">
              <Button className="px-8 py-4 text-lg font-semibold bg-gradient-to-r from-indigo-600 to-purple-600 hover:from-indigo-700 hover:to-purple-700 text-white rounded-xl shadow-lg transform hover:scale-105 transition">
                Find Projects
              </Button>
            </Link>
            <Link to="/freelancer/dashboard">
              <Button variant="outline" className="px-8 py-4 text-lg font-semibold border-2 border-indigo-600 text-indigo-600 hover:bg-indigo-50 rounded-xl">
                My Dashboard
              </Button>
            </Link>
          </div>
        </div>
      </section>

      {/* Testimonials */}
      <section className="py-20 bg-gray-100">
        <div className="max-w-7xl mx-auto px-4">
          <h2 className="text-3xl font-bold text-center text-gray-900">What our freelancers say</h2>
          <p className="mt-4 text-center text-gray-600">Thousands of freelancers trust Connect. Read their testimonials!</p>
          <div className="mt-12 grid grid-cols-1 md:grid-cols-3 gap-8">
            {testimonials.map((t, i) => (
              <div key={i} className="bg-white p-6 rounded-xl shadow-md">
                <div className="flex mb-4">
                  {[...Array(t.rating)].map((_, j) => (
                    <span key={j} className="text-yellow-400 text-xl">Star</span>
                  ))}
                </div>
                <p className="text-gray-700 italic">"{t.text}"</p>
                <div className="mt-6 flex items-center">
                  <div className="w-12 h-12 bg-gradient-to-br from-indigo-400 to-purple-400 rounded-full" />
                  <div className="ml-3">
                    <p className="font-semibold text-gray-900">{t.name}</p>
                    <p className="text-sm text-gray-500">{t.role}</p>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Categories */}
      <section className="py-20">
        <div className="max-w-7xl mx-auto px-4">
          <h2 className="text-3xl font-bold text-center text-gray-900">
            Explore projects in any category
          </h2>
          <p className="mt-4 text-center text-lg text-gray-600">Основные категории IT-проектов</p>
          <div className="mt-12 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {categories.map((cat, i) => (
              <Link
                key={i}
                to="/freelancer/projects"
                className="group p-6 bg-white rounded-2xl shadow-lg hover:shadow-2xl transition transform hover:-translate-y-1 cursor-pointer"
              >
                <div className={`w-16 h-16 rounded-xl bg-gradient-to-br ${cat.color} flex items-center justify-center text-3xl mb-4`}>
                  {cat.icon}
                </div>
                <h3 className="text-xl font-semibold text-gray-900 group-hover:text-indigo-600 transition">
                  {cat.title}
                </h3>
              </Link>
            ))}
          </div>
        </div>
      </section>

      <Footer />
    </div>
  );
}