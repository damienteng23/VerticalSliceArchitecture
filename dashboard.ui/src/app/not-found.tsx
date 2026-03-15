import { notFound } from 'next/navigation';

// not found page for all routes that don't start with /about, /blog, or /products
export default async function NotFound() {
  return (
    <div>
        <h2>Page Not Found</h2>
        <p>The page you are looking for does not exist.</p>
    </div>
  );
}