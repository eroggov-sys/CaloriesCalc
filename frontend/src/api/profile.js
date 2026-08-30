import { authorizedFetch } from "@/api/auth"

const API_URL = "http://localhost:5077/api"

export async function getProfile() {
    const response = await authorizedFetch(`${API_URL}/profile`)    

     if (response.status === 404) {
        return null
    }

    if (!response.ok) {
        throw new Error("Failed to load profile")
    }
    return response.json()
}

export async function updateProfile(profile) {
  const response = await authorizedFetch(
    `${API_URL}/profile`,
    {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(profile),
    },
  )

  if (!response.ok) {
    throw new Error("Failed to save profile")
  }

  return response.json()
}

export async function getNutritionTargets() {
  const response = await authorizedFetch(
    `${API_URL}/profile/targets`,
  )

  if (response.status === 404) {
    return null
  }

  if (!response.ok) {
    throw new Error("Failed to load nutrition targets")
  }

  return response.json()
}