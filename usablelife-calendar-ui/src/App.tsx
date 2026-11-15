import { Routes, Route, Navigate } from 'react-router-dom'
import Login from './pages/Login'
import Calendar from './pages/Calendar'
import SubmitEvent from './pages/SubmitEvent'
import PendingEvents from './pages/PendingEvents'
import Users from './pages/Users'
import ManageEvents from './pages/ManageEvents'
import Layout from './components/Layout'
import ProtectedRoute from './components/ProtectedRoute'

function App() {
  return (
    <Routes>
      <Route path="/" element={<Navigate to="/login" replace />} />
      <Route path="/login" element={<Login />} />
      <Route element={<Layout />}>
        <Route path="/calendar" element={<ProtectedRoute><Calendar /></ProtectedRoute>} />
        <Route path="/calendar/day" element={<ProtectedRoute><Calendar /></ProtectedRoute>} />
        <Route path="/submit-event" element={<ProtectedRoute><SubmitEvent /></ProtectedRoute>} />
        <Route path="/pending-events" element={<ProtectedRoute approverOnly><PendingEvents /></ProtectedRoute>} />
        <Route path="/users" element={<ProtectedRoute approverOnly><Users /></ProtectedRoute>} />
        <Route path="/manage-events" element={<ProtectedRoute approverOnly><ManageEvents /></ProtectedRoute>} />
      </Route>
      <Route path="*" element={<Navigate to="/login" replace />} />
    </Routes>
  )
}

export default App
