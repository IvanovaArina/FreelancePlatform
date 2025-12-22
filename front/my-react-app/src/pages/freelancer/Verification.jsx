// src/pages/freelancer/Verification.jsx
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { ActionButton, Input } from '../../components/ui';

export default function Verification() {
  const navigate = useNavigate();
  const [step, setStep] = useState(1); // 1: upload, 2: form
  const [passportPhoto, setPassportPhoto] = useState(null);
  const [selfiePhoto, setSelfiePhoto] = useState(null);
  const [previewPassport, setPreviewPassport] = useState(null);
  const [previewSelfie, setPreviewSelfie] = useState(null);

  const [formData, setFormData] = useState({
    lastName: '',
    firstName: '',
    passportNumber: '',
    dob: '',
  });

  const [errors, setErrors] = useState({});

  // Simulate OCR (in real app — via API)
  const simulateOCR = () => {
    setTimeout(() => {
      setFormData({
        lastName: 'Ivanov',
        firstName: 'Alexey',
        passportNumber: 'AB1234567',
        dob: '15/03/1990',
      });
      alert('OCR completed! Data extracted (simulation)');
    }, 1500);
 O  };

  const handleFileChange = (e, type) => {
    const file = e.target.files[0];
    if (!file) return;

    if (file.size > 5 * 1024 * 1024) {
      alert('File size exceeds 5 MB');
      return;
    }

    if (!['image/jpeg', 'image/jpg', 'image/png'].includes(file.type)) {
      alert('Only JPG or PNG files are allowed');
      return;
    }

    const reader = new FileReader();
    reader.onloadend = () => {
      if (type === 'passport') {
        setPassportPhoto(file);
        setPreviewPassport(reader.result);
        simulateOCR();
      } else {
        setSelfiePhoto(file);
        setPreviewSelfie(reader.result);
      }
    };
    reader.readAsDataURL(file);
  };

  const validateForm = () => {
    const newErrors = {};
    if (!formData.lastName.trim()) newErrors.lastName = 'Required';
    if (!formData.firstName.trim()) newErrors.firstName = 'Required';
    if (!/^[A-Z]{2}\d{7}$/.test(formData.passportNumber))
      newErrors.passportNumber = 'Format: AB1234567';
    if (!/^\d{2}\/\d{2}\/\d{4}$/.test(formData.dob))
      newErrors.dob = 'Format: DD/MM/YYYY';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = () => {
    if (!validateForm()) return;
    if (!passportPhoto || !selfiePhoto) {
      alert('Please upload both photos');
      return;
    }
    alert('Verification submitted for review!');
    navigate('/freelancer/profile');
  };

  return (
    <div className="max-w-4xl mx-auto py-12 px-4">
      <div className="text-center mb-10">
        <h1 className="text-3xl font-bold text-gray-900">Identity Verification</h1>
        <p className="text-gray-600 mt-2">Verify your identity to earn the "Verified" badge</p>
      </div>

      <div className="bg-white rounded-2xl shadow-lg border border-gray-200 p-8">
        {/* Steps */}
        <div className="flex items-center justify-center mb-10">
          <div className={`flex items-center ${step === 1 ? 'text-indigo-600' : 'text-gray-400'}`}>
            <div className="w-10 h-10 rounded-full bg-indigo-600 text-white flex items-center justify-center font-bold">1</div>
            <span className="ml-3 text-lg font-medium">Upload Documents</span>
          </div>
          <div className="w-24 h-1 bg-gray-300 mx-6" />
          <div className={`flex items-center ${step === 2 ? 'text-indigo-600' : 'text-gray-400'}`}>
            <div className={`w-10 h-10 rounded-full ${step === 2 ? 'bg-indigo-600 text-white' : 'bg-gray-300'} flex items-center justify-center font-bold`}>2</div>
            <span className="ml-3 text-lg font-medium">Confirm Details</span>
          </div>
        </div>

        {/* STEP 1: Upload Photos */}
        {step === 1 && (
          <div className="space-y-8">
            {/* Passport Photo */}
            <div>
              <h3 className="text-xl font-semibold mb-4">Passport Photo (open page with photo)</h3>
              <div className="border-2 border-dashed border-gray-300 rounded-xl p-8 text-center">
                {previewPassport ? (
                  <div>
                    <img src={previewPassport} alt="Passport" className="max-w-full max-h-96 mx-auto rounded-lg shadow-md" />
                    <p className="mt-4 text-green-600 font-medium">Uploaded</p>
                  </div>
                ) : (
                  <>
                    <svg className="w-16 h-16 mx-auto text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                    </svg>
                    <p className="mt-4 text-gray-600">Drag & drop or click to upload</p>
                    <p className="text-sm text-gray-500">JPG, PNG up to 5 MB</p>
                    <input
                      type="file"
                      accept="image/jpeg,image/png"
                      onChange={(e) => handleFileChange(e, 'passport')}
                      className="mt-4 block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-indigo-50 file:text-indigo-700 hover:file:bg-indigo-100"
                    />
                  </>
                )}
              </div>
            </div>

            {/* Selfie with Passport */}
            <div>
              <h3 className="text-xl font-semibold mb-4">Selfie Holding Your Passport</h3>
              <div className="border-2 border-dashed border-gray-300 rounded-xl p-8 text-center">
                {previewSelfie ? (
                  <div>
                    <img src={previewSelfie} alt="Selfie" className="max-w-full max-h-96 mx-auto rounded-lg shadow-md" />
                    <p className="mt-4 text-green-600 font-medium">Uploaded</p>
                  </div>
                ) : (
                  <>
                    <svg className="w-16 h-16 mx-auto text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                    </svg>
                    <p className="mt-4 text-gray-600">Hold your passport next to your face</p>
                    <input
                      type="file"
                      accept="image/jpeg,image/png"
                      onChange={(e) => handleFileChange(e, 'selfie')}
                      className="mt-4 block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-indigo-50 file:text-indigo-700 hover:file:bg-indigo-100"
                    />
                  </>
                )}
              </div>
            </div>

            <div className="flex justify-end">
              <ActionButton
                size="lg"
                onClick={() => setStep(2)}
                disabled={!passportPhoto || !selfiePhoto}
              >
                Next
              </ActionButton>
            </div>
          </div>
        )}

        {/* STEP 2: Confirm Data */}
        {step === 2 && (
          <div className="space-y-6">
            <div className="grid grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Last Name (as in passport)</label>
                <Input
                  value={formData.lastName}
                  onChange={(e) => setFormData({ ...formData, lastName: e.target.value })}
                  placeholder="Ivanov"
                  error={errors.lastName}
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">First Name (as in passport)</label>
                <Input
                  value={formData.firstName}
                  onChange={(e) => setFormData({ ...formData, firstName: e.target.value })}
                  placeholder="Alexey"
                  error={errors.firstName}
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Passport Series & Number</label>
              <Input
                value={formData.passportNumber}
                onChange={(e) => setFormData({ ...formData, passportNumber: e.target.value.toUpperCase() })}
                placeholder="AB1234567"
                error={errors.passportNumber}
              />
              <p className="text-xs text-gray-500 mt-1">Format: two letters + 7 digits</p>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Date of Birth</label>
              <Input
                value={formData.dob}
                onChange={(e) => setFormData({ ...formData, dob: e.target.value })}
                placeholder="15/03/1990"
                error={errors.dob}
              />
            </div>

            <div className="flex justify-between">
              <ActionButton variant="outline" onClick={() => setStep(1)}>
                Back
              </ActionButton>
              <ActionButton size="lg" onClick={handleSubmit}>
                Submit for Review
              </ActionButton>
            </div>
          </div>
        )}
      </div>

      {/* Info */}
      <div className="mt-8 text-center text-sm text-gray-500">
        <p>Verification usually takes up to 24 hours</p>
        <p>We never share your personal data with third parties</p>
      </div>
    </div>
  );
}