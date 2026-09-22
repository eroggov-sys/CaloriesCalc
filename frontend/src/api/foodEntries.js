import { authorizedFetch } from "@/api/auth"
import { readErrorMessage } from "@/api/problem"
import { API_URL } from "@/api/config"



export async function getDiaryDay(date) {

  const response = await authorizedFetch(
    `${API_URL}/diary?date=${encodeURIComponent(date)}`,
  )

  if (!response.ok) {
    throw new Error(await readErrorMessage(response, "Failed to load diary"))
  }

  return response.json()
}

export async function createFoodEntry( entry) {

  const response = await authorizedFetch( `${API_URL}/FoodEntries`,
    {
      method: "POST",
      headers: { 
        "Content-Type": "application/json",
      },
      body: JSON.stringify(entry),
    },
  )

  if (!response.ok) {
    throw new Error(await readErrorMessage(response, "Failed to create food entry"))
  }
  return response.json()
}

export async function deleteFoodEntry(id) {
  const response = await authorizedFetch(`${API_URL}/FoodEntries/${id}`, {
    method: "DELETE",
  })

  if (!response.ok) {
    throw new Error(await readErrorMessage(response, "Failed to delete food entry"))
  } 

}

export async function updateFoodEntry(id, quantityGrams) {
  const response = await authorizedFetch(`${API_URL}/FoodEntries/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ quantityGrams }),
  })

  if (!response.ok) {
    throw new Error(await readErrorMessage(response, "Failed to update food entry"))

  }

  return response.json()
}

