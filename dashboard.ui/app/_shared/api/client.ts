const BASE_URL = process.env.API_URL ?? "https://localhost:7212";

export async function apiFetch<T>(path: string, options?: RequestInit): Promise<T> {
    const res = await fetch(`${BASE_URL}${path}`, {
        headers: { "Content-Type": "application/json" },
        ...options,
    });

    if (!res.ok) throw new Error(`API error ${res.status}: ${path}`);

    return res.json() as Promise<T>;
}
