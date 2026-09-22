import { authorizedFetch } from "@/api/auth"
import { readErrorMessage } from "@/api/problem"
import { API_URL } from "@/api/config"


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

export async function importFood(source, externalId) {
    const response = await authorizedFetch(`${API_URL}/Food/import`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ source, externalId }),
    })

    if (!response.ok) {
        throw new Error(await readErrorMessage(response, "Failed to import food"))
    }

    return response.json()
}

export async function findFoodByBarcode(barcode) {
    const response = await authorizedFetch(
        `${API_URL}/Food/barcode/${encodeURIComponent(barcode)}`,
    )

    if (response.status === 404) return null

    if (!response.ok) {
        throw new Error(await readErrorMessage(response, "Failed to look up barcode"))
    }

    return response.json()
}
