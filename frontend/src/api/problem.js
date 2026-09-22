export async function readErrorMessage(response, fallbackMessage) {
    const problem = await response.json().catch(() => null)

    if (problem?.errors) return Object
        .values(problem.errors)
        .flat()
        .join(" ")
    

    return problem?.detail ?? problem?.title ?? fallbackMessage
}