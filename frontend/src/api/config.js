const BASE_URL = import.meta.env.VITE_API_URL ?? "http://localhost:5077"

export const IDENTITY_URL = BASE_URL          // /login, /register, /refresh
export const API_URL = `${BASE_URL}/api`      // controllers
