import Header from './components/Header/Header.jsx';
import Register from './pages/Register/Register.jsx';
import Login from './pages/Login/Login.jsx';
import Home from './pages/Home/Home.jsx';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css'
import { Toaster } from 'react-hot-toast';
import ChangeRole from "./pages/ChangeRole/ChangeRole.jsx";
import Backup from "./pages/Backup/Backup.jsx";
import Profile from "./pages/Profile/Profile.jsx";
import EditProfile from "./pages/EditProfile/EditProfile.jsx";

function App() {

  return (
    <div>
      <Router>
        <Toaster />
        <Header />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<Login />} />
          <Route path="/change-role" element={<ChangeRole />} />
          <Route path="/backup" element={<Backup />} />
          <Route path="/profile" element={<Profile />} />
          <Route path="/edit-profile" element={<EditProfile />} />
        </Routes>
      </Router>
    </div>
  )
}

export default App
