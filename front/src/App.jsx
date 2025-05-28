import React, { useState, useEffect } from 'react';
import { User, LogIn, LogOut, Plus, CreditCard, UserPlus, Settings, Briefcase, DollarSign } from 'lucide-react';

const API_BASE = 'http://localhost:5048/api'; // Замените на ваш URL API

// Утилиты для работы с токеном
const getToken = () => localStorage.getItem('token');
const setToken = (token) => localStorage.setItem('token', token);
const removeToken = () => localStorage.removeItem('token');
const getUserRole = () => {
  const token = getToken();
  if (!token) return null;
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || payload.role;
  } catch {
    return null;
  }
};
const getUserId = () => {
  const token = getToken();
  if (!token) return null;
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload['http://schemas.xmlsoap.org/2004/09/identity/claims/nameidentifier'] || payload.sub;
  } catch {
    return null;
  }
};

// API функции
const api = {
  // Аутентификация
  register: async (role, email, password, name) => {
    const response = await fetch(`${API_BASE}/auth/register/${role}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password, name })
    });
    return response.json();
  },
  
  login: async (email, password) => {
    const response = await fetch(`${API_BASE}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });
    return response.json();
  },
  
  // Заказы
  createOrder: async (type, title, basePrice) => {
    const response = await fetch(`${API_BASE}/orders/create/${type}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${getToken()}`
      },
      body: JSON.stringify({ title, basePrice })
    });
    return response.json();
  },
  
  acceptOrder: async (orderId) => {
    const response = await fetch(`${API_BASE}/orders/${orderId}/accept`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${getToken()}`
      }
    });
    return response.json();
  },
  
  cloneOrder: async (orderId) => {
    const response = await fetch(`${API_BASE}/orders/clone/${orderId}`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${getToken()}`
      }
    });
    return response.json();
  },
  
  getActiveOrders: async () => {
    const response = await fetch(`${API_BASE}/orders/active`, {
      headers: {
        'Authorization': `Bearer ${getToken()}`
      }
    });
    return response.json();
  },
  
  enhanceOrder: async (orderId, isUrgent, hasPremiumSupport) => {
    const response = await fetch(`${API_BASE}/orders/enhance/${orderId}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${getToken()}`
      },
      body: JSON.stringify({ isUrgent, hasPremiumSupport })
    });
    return response.json();
  },
  
  // Платежи
  createAndPayOrder: async (type, paymentType, title, basePrice, hours) => {
    const response = await fetch(`${API_BASE}/payments/create-and-pay/${type}/${paymentType}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${getToken()}`
      },
      body: JSON.stringify({ title, basePrice, hours })
    });
    return response.json();
  },
  
  // Профили
  createProfile: async (name, skill, portfolioItem, review) => {
    const response = await fetch(`${API_BASE}/profiles/create`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${getToken()}`
      },
      body: JSON.stringify({ name, skill, portfolioItem, review })
    });
    return response.json();
  },
  
  // Пользователи
  createUser: async (role, email, name) => {
    const response = await fetch(`${API_BASE}/users/create/${role}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, name })
    });
    return response.json();
  }
};

// Компонент авторизации
const AuthForm = ({ onLogin }) => {
  const [isLogin, setIsLogin] = useState(true);
  const [role, setRole] = useState('Client');
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    name: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    
    try {
      if (isLogin) {
        const result = await api.login(formData.email, formData.password);
        if (result.token) {
          setToken(result.token);
          onLogin(result.token);
        } else {
          setError('Неверные учетные данные');
        }
      } else {
        await api.register(role, formData.email, formData.password, formData.name);
        setIsLogin(true);
        setFormData({ email: '', password: '', name: '' });
        alert('Регистрация успешна! Теперь войдите в систему.');
      }
    } catch (err) {
      setError('Произошла ошибка. Попробуйте снова.');
    }
    
    setLoading(false);
  };

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        <div>
          <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
            {isLogin ? 'Вход в систему' : 'Регистрация'}
          </h2>
        </div>
        <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
          {error && (
            <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
              {error}
            </div>
          )}
          
          <div className="space-y-4">
            {!isLogin && (
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  Роль
                </label>
                <select
                  value={role}
                  onChange={(e) => setRole(e.target.value)}
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                >
                  <option value="Client">Клиент</option>
                  <option value="Freelancer">Фрилансер</option>
                  <option value="Admin">Администратор</option>
                </select>
              </div>
            )}
            
            {!isLogin && (
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  Имя
                </label>
                <input
                  type="text"
                  required
                  value={formData.name}
                  onChange={(e) => setFormData({...formData, name: e.target.value})}
                  className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
                />
              </div>
            )}
            
            <div>
              <label className="block text-sm font-medium text-gray-700">
                Email
              </label>
              <input
                type="email"
                required
                value={formData.email}
                onChange={(e) => setFormData({...formData, email: e.target.value})}
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
              />
            </div>
            
            <div>
              <label className="block text-sm font-medium text-gray-700">
                Пароль
              </label>
              <input
                type="password"
                required
                value={formData.password}
                onChange={(e) => setFormData({...formData, password: e.target.value})}
                className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
              />
            </div>
          </div>

          <div>
            <button
              type="submit"
              disabled={loading}
              className="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 disabled:opacity-50"
            >
              <LogIn className="w-5 h-5 mr-2" />
              {loading ? 'Загрузка...' : (isLogin ? 'Войти' : 'Зарегистрироваться')}
            </button>
          </div>
          
          <div className="text-center">
            <button
              type="button"
              onClick={() => setIsLogin(!isLogin)}
              className="text-indigo-600 hover:text-indigo-500"
            >
              {isLogin ? 'Нужна регистрация?' : 'Уже есть аккаунт?'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

// Компонент дашборда для клиентов
const ClientDashboard = () => {
  const [orders, setOrders] = useState([]);
  const [showCreateOrder, setShowCreateOrder] = useState(false);
  const [showPaymentForm, setShowPaymentForm] = useState(false);

  const CreateOrderForm = () => {
    const [formData, setFormData] = useState({
      type: 'design',
      title: '',
      basePrice: ''
    });

    const handleSubmit = async (e) => {
      e.preventDefault();
      try {
        const result = await api.createOrder(formData.type, formData.title, parseFloat(formData.basePrice));
        setOrders([...orders, result]);
        setFormData({ type: 'design', title: '', basePrice: '' });
        setShowCreateOrder(false);
        alert('Заказ создан успешно!');
      } catch (error) {
        alert('Ошибка создания заказа');
      }
    };

    return (
      <div className="bg-white p-6 rounded-lg shadow-md">
        <h3 className="text-lg font-medium mb-4">Создать новый заказ</h3>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Тип заказа</label>
            <select
              value={formData.type}
              onChange={(e) => setFormData({...formData, type: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            >
              <option value="design">Дизайн</option>
              <option value="coding">Программирование</option>
              <option value="marketing">Маркетинг</option>
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Название</label>
            <input
              type="text"
              required
              value={formData.title}
              onChange={(e) => setFormData({...formData, title: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Базовая цена</label>
            <input
              type="number"
              required
              value={formData.basePrice}
              onChange={(e) => setFormData({...formData, basePrice: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            />
          </div>
          <div className="flex space-x-2">
            <button
              type="submit"
              className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
              Создать заказ
            </button>
            <button
              type="button"
              onClick={() => setShowCreateOrder(false)}
              className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400"
            >
              Отмена
            </button>
          </div>
        </form>
      </div>
    );
  };

  const PaymentForm = () => {
    const [formData, setFormData] = useState({
      type: 'design',
      paymentType: 'stripe',
      title: '',
      basePrice: '',
      hours: ''
    });

    const handleSubmit = async (e) => {
      e.preventDefault();
      try {
        const result = await api.createAndPayOrder(
          formData.type,
          formData.paymentType,
          formData.title,
          parseFloat(formData.basePrice),
          parseInt(formData.hours)
        );
        alert('Заказ создан и оплачен успешно!');
        setShowPaymentForm(false);
        setFormData({ type: 'design', paymentType: 'stripe', title: '', basePrice: '', hours: '' });
      } catch (error) {
        alert('Ошибка создания заказа с оплатой');
      }
    };

    return (
      <div className="bg-white p-6 rounded-lg shadow-md">
        <h3 className="text-lg font-medium mb-4">Создать заказ с оплатой</h3>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Тип заказа</label>
            <select
              value={formData.type}
              onChange={(e) => setFormData({...formData, type: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            >
              <option value="design">Дизайн</option>
              <option value="coding">Программирование</option>
              <option value="marketing">Маркетинг</option>
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Способ оплаты</label>
            <select
              value={formData.paymentType}
              onChange={(e) => setFormData({...formData, paymentType: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            >
              <option value="stripe">Stripe</option>
              <option value="paypal">PayPal</option>
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Название</label>
            <input
              type="text"
              required
              value={formData.title}
              onChange={(e) => setFormData({...formData, title: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Базовая цена</label>
            <input
              type="number"
              required
              value={formData.basePrice}
              onChange={(e) => setFormData({...formData, basePrice: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Количество часов</label>
            <input
              type="number"
              required
              value={formData.hours}
              onChange={(e) => setFormData({...formData, hours: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            />
          </div>
          <div className="flex space-x-2">
            <button
              type="submit"
              className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
            >
              <CreditCard className="w-4 h-4 inline mr-2" />
              Создать и оплатить
            </button>
            <button
              type="button"
              onClick={() => setShowPaymentForm(false)}
              className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400"
            >
              Отмена
            </button>
          </div>
        </form>
      </div>
    );
  };

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h2 className="text-2xl font-bold text-gray-900">Панель клиента</h2>
        <div className="space-x-2">
          <button
            onClick={() => setShowCreateOrder(true)}
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
            <Plus className="w-4 h-4 inline mr-2" />
            Создать заказ
          </button>
          <button
            onClick={() => setShowPaymentForm(true)}
            className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
          >
            <CreditCard className="w-4 h-4 inline mr-2" />
            Заказ с оплатой
          </button>
        </div>
      </div>

      {showCreateOrder && <CreateOrderForm />}
      {showPaymentForm && <PaymentForm />}

      <div className="bg-white p-6 rounded-lg shadow-md">
        <h3 className="text-lg font-medium mb-4">Мои заказы</h3>
        {orders.length === 0 ? (
          <p className="text-gray-500">У вас пока нет заказов</p>
        ) : (
          <div className="space-y-4">
            {orders.map((order, index) => (
              <div key={index} className="border p-4 rounded">
                <h4 className="font-medium">{order.title}</h4>
                <p className="text-gray-600">Цена: ${order.basePrice}</p>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

// Компонент дашборда для фрилансеров
const FreelancerDashboard = () => {
  const [showCreateProfile, setShowCreateProfile] = useState(false);
  const [activeOrders, setActiveOrders] = useState([]);

  const CreateProfileForm = () => {
    const [formData, setFormData] = useState({
      name: '',
      skill: '',
      portfolioItem: '',
      review: ''
    });

    const handleSubmit = async (e) => {
      e.preventDefault();
      try {
        await api.createProfile(formData.name, formData.skill, formData.portfolioItem, formData.review);
        alert('Профиль создан успешно!');
        setShowCreateProfile(false);
        setFormData({ name: '', skill: '', portfolioItem: '', review: '' });
      } catch (error) {
        alert('Ошибка создания профиля');
      }
    };

    return (
      <div className="bg-white p-6 rounded-lg shadow-md">
        <h3 className="text-lg font-medium mb-4">Создать профиль</h3>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Имя</label>
            <input
              type="text"
              required
              value={formData.name}
              onChange={(e) => setFormData({...formData, name: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Навыки</label>
            <input
              type="text"
              required
              value={formData.skill}
              onChange={(e) => setFormData({...formData, skill: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Портфолио</label>
            <textarea
              required
              value={formData.portfolioItem}
              onChange={(e) => setFormData({...formData, portfolioItem: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Отзыв</label>
            <textarea
              required
              value={formData.review}
              onChange={(e) => setFormData({...formData, review: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            />
          </div>
          <div className="flex space-x-2">
            <button
              type="submit"
              className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
              Создать профиль
            </button>
            <button
              type="button"
              onClick={() => setShowCreateProfile(false)}
              className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400"
            >
              Отмена
            </button>
          </div>
        </form>
      </div>
    );
  };

  const handleAcceptOrder = async (orderId) => {
    try {
      await api.acceptOrder(orderId);
      alert('Заказ принят успешно!');
      // Обновить список заказов
    } catch (error) {
      alert('Ошибка принятия заказа');
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h2 className="text-2xl font-bold text-gray-900">Панель фрилансера</h2>
        <button
          onClick={() => setShowCreateProfile(true)}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          <User className="w-4 h-4 inline mr-2" />
          Создать профиль
        </button>
      </div>

      {showCreateProfile && <CreateProfileForm />}

      <div className="bg-white p-6 rounded-lg shadow-md">
        <h3 className="text-lg font-medium mb-4">Доступные заказы</h3>
        <p className="text-gray-500">Здесь будут отображаться доступные заказы для принятия</p>
      </div>
    </div>
  );
};

// Компонент дашборда для администраторов
const AdminDashboard = () => {
  const [activeOrders, setActiveOrders] = useState([]);
  const [showCreateUser, setShowCreateUser] = useState(false);

  useEffect(() => {
    loadActiveOrders();
  }, []);

  const loadActiveOrders = async () => {
    try {
      const orders = await api.getActiveOrders();
      setActiveOrders(orders);
    } catch (error) {
      console.error('Ошибка загрузки активных заказов');
    }
  };

  const CreateUserForm = () => {
    const [formData, setFormData] = useState({
      role: 'Client',
      email: '',
      name: ''
    });

    const handleSubmit = async (e) => {
      e.preventDefault();
      try {
        await api.createUser(formData.role, formData.email, formData.name);
        alert('Пользователь создан успешно!');
        setShowCreateUser(false);
        setFormData({ role: 'Client', email: '', name: '' });
      } catch (error) {
        alert('Ошибка создания пользователя');
      }
    };

    return (
      <div className="bg-white p-6 rounded-lg shadow-md">
        <h3 className="text-lg font-medium mb-4">Создать пользователя</h3>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700">Роль</label>
            <select
              value={formData.role}
              onChange={(e) => setFormData({...formData, role: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            >
              <option value="Client">Клиент</option>
              <option value="Freelancer">Фрилансер</option>
              <option value="Admin">Администратор</option>
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Email</label>
            <input
              type="email"
              required
              value={formData.email}
              onChange={(e) => setFormData({...formData, email: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700">Имя</label>
            <input
              type="text"
              required
              value={formData.name}
              onChange={(e) => setFormData({...formData, name: e.target.value})}
              className="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md"
            />
          </div>
          <div className="flex space-x-2">
            <button
              type="submit"
              className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
              Создать пользователя
            </button>
            <button
              type="button"
              onClick={() => setShowCreateUser(false)}
              className="bg-gray-300 text-gray-700 px-4 py-2 rounded hover:bg-gray-400"
            >
              Отмена
            </button>
          </div>
        </form>
      </div>
    );
  };

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h2 className="text-2xl font-bold text-gray-900">Панель администратора</h2>
        <button
          onClick={() => setShowCreateUser(true)}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          <UserPlus className="w-4 h-4 inline mr-2" />
          Создать пользователя
        </button>
      </div>

      {showCreateUser && <CreateUserForm />}

      <div className="bg-white p-6 rounded-lg shadow-md">
        <h3 className="text-lg font-medium mb-4">Активные заказы</h3>
        {activeOrders.length === 0 ? (
          <p className="text-gray-500">Нет активных заказов</p>
        ) : (
          <div className="space-y-4">
            {activeOrders.map((order, index) => (
              <div key={index} className="border p-4 rounded">
                <h4 className="font-medium">{order.title}</h4>
                <p className="text-gray-600">Цена: ${order.basePrice}</p>
                <p className="text-gray-600">Статус: {order.status}</p>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

// Главный компонент приложения
const App = () => {
  const [user, setUser] = useState(null);
  const [userRole, setUserRole] = useState(null);

  useEffect(() => {
    const token = getToken();
    if (token) {
      setUser(token);
      setUserRole(getUserRole());
    }
  }, []);

  const handleLogin = (token) => {
    setUser(token);
    setUserRole(getUserRole());
  };

  const handleLogout = () => {
    removeToken();
    setUser(null);
    setUserRole(null);
  };

  if (!user) {
    return <AuthForm onLogin={handleLogin} />;
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Навигационная панель */}
      <nav className="bg-white shadow-sm border-b">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between h-16">
            <div className="flex items-center">
              <Briefcase className="h-8 w-8 text-indigo-600" />
              <span className="ml-2 text-xl font-semibold text-gray-900">
                Freelance Platform
              </span>
            </div>
            <div className="flex items-center space-x-4">
              <span className="text-sm text-gray-700">
                Роль: {userRole === 'Client' ? 'Клиент' : userRole === 'Freelancer' ? 'Фрилансер' : 'Администратор'}
              </span>
              <button
                onClick={handleLogout}
                className="inline-flex items-center px-3 py-2 border border-transparent text-sm leading-4 font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
              >
                <LogOut className="h-4 w-4 mr-2" />
                Выйти
              </button>
            </div>
          </div>
        </div>
      </nav>

      {/* Основной контент */}
      <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        <div className="px-4 py-6 sm:px-0">
          {userRole === 'Client' && <ClientDashboard />}
          {userRole === 'Freelancer' && <FreelancerDashboard />}
          {userRole === 'Admin' && <AdminDashboard />}
        </div>
      </main>
    </div>
  );
};

export default App;