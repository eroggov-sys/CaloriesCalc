import { useEffect, useState } from "react"
import { isAuthenticated, logout } from "@/api/auth"
import { getProfile } from "@/api/profile"
import DashBoard from "./components/DashBoard"
import LoginForm from "./components/LoginForm"
import RegisterForm from "./components/RegisterForm"
import ProfileForm from "./components/ProfileForm"
import { Button } from "@/components/ui/button"


function App() {
  const [authenticated, setAuthenticated] = useState(isAuthenticated)
  const [showRegister, setShowRegister] = useState(false)
  const [profile, setProfile] = useState(null)
  const [isProfileLoading, setIsProfileLoading] = useState(false)
  const [profileError, setProfileError] = useState("")
  const [isEditingProfile, setIsEditingProfile] = useState(false)

  useEffect(() => {
    function handleAuthLogout() {
      setAuthenticated(false)
      setProfile(null)
    }

    window.addEventListener("auth:logout", handleAuthLogout)

    return () => {
      window.removeEventListener("auth:logout", handleAuthLogout)
    }
  }, [])

  useEffect(() => {
    if (!authenticated) return

    let canceled = false

    async function loadProfile() {
      try {
        setIsProfileLoading(true)
        setProfileError("")

        const data = await getProfile()
        if (!canceled) setProfile(data)

      } catch (error) {

        if (!canceled) setProfileError(error.message)        
      } finally {

        if (!canceled) setIsProfileLoading(false)
      }
    }

    loadProfile()
    return () => {
      canceled = true
    }
  }, [authenticated])

  function handleLogin() {
    setAuthenticated(true)
  }

  function handleLogout() {
    logout()
    setAuthenticated(false)
    setProfile(null)
    setIsEditingProfile(false)
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

  if (isProfileLoading) {
    return (
      <main className="grid min-h-screen place-items-center">
        <p className="text-sm text-zinc-500">
          Loading profile...
        </p>
      </main>
    )
  }

  if (profileError) {
    return (
      <main className="grid min-h-screen place-items-center p-6">
        <div className="space-y-4 text-center">
          <p className="text-sm text-red-600">
            {profileError}
          </p>

          <Button
            type="button"
            variant="outline"
            onClick={handleLogout}
          >
            Sign out
          </Button>
        </div>
      </main>
    )
  }

  if (!profile || isEditingProfile) {
  return (
      <main className="grid min-h-screen place-items-center bg-zinc-50 p-6">
        <ProfileForm
          initialProfile={profile}
          onSaved={(savedProfile) => {
            setProfile(savedProfile)
            setIsEditingProfile(false)
          }}
          onCancel={
            profile
              ? () => setIsEditingProfile(false)
              : undefined
          }
        />
      </main>
    )
  }


  return (
      <div>
        <DashBoard 
          profile = {profile}
          onEditProfile = {() => setIsEditingProfile(true)}
          onLogout={handleLogout} 
        />
      </div>
  )
}

export default App
