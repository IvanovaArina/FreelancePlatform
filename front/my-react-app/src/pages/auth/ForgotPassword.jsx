import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Input } from '../../components/ui';
import AuthLayout from '../../components/layout/AuthLayout';

const ForgotPasswordPage = () => {
  const navigate = useNavigate();
  const [email, setEmail] = React.useState('');

  const handleSubmit = () => {
    console.log('Password reset requested for:', email);
    alert('Password reset link sent to your email!');
  };

  return (
    <AuthLayout>
      <div>
        <h1 className="text-3xl font-bold text-gray-900 mb-2">
          Forgot Password?
        </h1>
        <p className="text-gray-600 text-sm">
          Enter your email address and we'll send you a link to reset your password
        </p>
        
        <div className="mt-8">
          <Input
            label="Email address"
            type="email"
            placeholder="Enter your email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          
          <Button onClick={handleSubmit}>
            Send Reset Link
          </Button>
          
          <div className="mt-6 text-center text-sm">
            <button
              onClick={() => navigate('/signin')}
              className="text-indigo-600 font-medium hover:underline"
            >
              ← Back to Sign In
            </button>
          </div>
        </div>
      </div>
    </AuthLayout>
  );
};

export default ForgotPasswordPage;