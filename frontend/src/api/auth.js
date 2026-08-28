const API_URL = "http://localhost:5077"

export async function register(email, password) {   
    const response = await fetch(`${API_URL}/register`,{

        method: "POST",
        headers: {"Content-Type":"application/json"},
        body: JSON.stringify({email, password}),
    })

    if (!response.ok) throw new Error("Failed to register")
}

export async function login(email, password) {
    const response = await fetch(`${API_URL}/login`,{
        method:"POST",
        headers: {"Content-Type" : "application/json"},
        body: JSON.stringify({email, password}),
    })

    if (!response.ok) throw new Error("Invalid email or password")
    
    const data = await response.json();

    localStorage.setItem("accessToken", data.accessToken)
    localStorage.setItem("refreshToken", data.refreshToken)

    return data
}

export function logout() {
    localStorage.removeItem("accessToken")
    localStorage.removeItem("refreshToken")

    window.dispatchEvent(new Event("auth:logout"))
}

export function getAccessToken() {
    return localStorage.getItem("accessToken")
}

export function isAuthenticated() {
    return Boolean(getAccessToken())
}

export async function refreshAccessToken() {

    const refreshToken = localStorage.getItem("refreshToken")

    if (!refreshToken) {
        logout()
        throw new Error("Session expired")
    }

    const response = await fetch(`${API_URL}/refresh`,{
        method: "POST",
        headers: {
        "Content-Type": "application/json",
        },
        body: JSON.stringify({ refreshToken }),
    })

    if (!response.ok) {
        logout()
        throw new Error("Session expired")
    }
    const data = await response.json()

    localStorage.setItem("accessToken", data.accessToken)
    localStorage.setItem("refreshToken", data.refreshToken)
    
    return data.accessToken
}

export async function authorizedFetch(url, options = {}) {
    let token = getAccessToken()

    if(!token) token = await refreshAccessToken()

    function sendRequest(accessToken) {
        return fetch(url, {
            ...options,
            headers:{
                ...options.headers,
                Authorization: `Bearer ${accessToken}`,
            },
        })
    }

    let response = await sendRequest(token)

    if (response.status === 401) {
        token = await refreshAccessToken()
        response = await sendRequest(token)
    }

    return response
}
