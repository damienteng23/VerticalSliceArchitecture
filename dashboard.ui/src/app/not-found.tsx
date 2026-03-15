"use client";
import { usePathname } from "next/navigation";

// not found page for all routes that don't start with /about, /blog, or /products
export default function NotFound() {
  const pathName = usePathname();
  const segments = pathName ? pathName.split("/").filter(Boolean) : [];

  const reviewId = segments[3] ? segments[3] : "null";
  const productId = segments[1] ? segments[1] : "null";

  return (
    <div>
      <h2>Page Not Found</h2>
      <p>
        The productID: {productId} & ReviewID: {reviewId} not found
      </p>
    </div>
  );
}
