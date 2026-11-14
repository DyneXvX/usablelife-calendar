import { Routes, Route } from 'react-router-dom'
import Login from './pages/Login'
import Calendar from './pages/Calendar'

function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/calendar" element={<Calendar />} />
    </Routes>
  )
}

export default App