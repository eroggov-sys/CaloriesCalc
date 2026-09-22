import { authorizedFetch } from "@/api/auth"
import { readErrorMessage } from "@/api/problem"

const API_URL = "http://localhost:5077/api"

export async function searchFoods(query, { page = 1, external = false } = {}) {
    const params = new URLSearchParams({
        query,
        page: String(page),
        external: String(external),
    })
    const response = await authorizedFetch(`${API_URL}/Food/search?${params}`)

    if (!response.ok) {
        throw new Error(await readErrorMessage(response, "Failed to search foods"))
    }

    return response.json()
}