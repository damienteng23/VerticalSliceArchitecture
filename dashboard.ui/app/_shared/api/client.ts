const BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? "https://localhost:7212";

export async function apiFetch<T>(path: string, options?: RequestInit): Promise<T> {
    const url = path.startsWith("/api/") ? path : `${BASE_URL}${path}`;
    const res = await fetch(url, {
        headers: { "Content-Type": "application/json" },
        ...options,
    });

    if (!res.ok) throw new Error(`API error ${res.status}: ${url}`);

    return res.json() as Promise<T>;
}
