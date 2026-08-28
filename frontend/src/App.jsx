
import { useEffect, useState } from "react"
import { isAuthenticated, logout } from "@/api/auth"
import DashBoard from "./components/DashBoard"
import LoginForm from "./components/LoginForm"
import RegisterForm from "./components/RegisterForm"

function App() {
  const [authenticated, setAuthenticated] = useState(isAuthenticated)
  const [showRegister, setShowRegister] = useState(false)

  useEffect(() => {
    function handleAuthLogout() {
      setAuthenticated(false)
    }

    window.addEventListener("auth:logout", handleAuthLogout)

    return () => {
      window.removeEventListener("auth:logout", handleAuthLogout)
    }
  }, [])

  function handleLogout() {
    logout()
    setAuthenticated(false)
  }

  if (!authenticated) {
    return (
      <main className="grid min-h-screen place-items-center p-6">
        {showRegister ? (
          <RegisterForm
            onRegistered={() => setShowRegister(false)}
            onShowLogin={() => setShowRegister(false)}
          />
        ) : (
          <LoginForm
            onLogin={() => setAuthenticated(true)}
            onShowRegister={() => setShowRegister(true)}
          />
        )}
      </main>
    )
}

  return (
      <div>
        <DashBoard onLogout={handleLogout} />
      </div>
  )
}

export default App
