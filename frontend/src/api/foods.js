import { authorizedFetch } from "@/api/auth";
const API_URL = "http://localhost:5077/api"

export async function searchFoods(query) {
    const response = await authorizedFetch(`${API_URL}/Food/search?query=${encodeURIComponent(query)}`,)

    if (!response.ok) throw new Error("Failed to search foods")

    return response.json()
}