import Link from "next/link";

export default function Products() {
  const productIds = [1, 2, 3, 4, 5];

  return (
    <>
      <h1>products page</h1>
      <Link href="/">/home</Link>

      <ul>
        {productIds.map((id) => (
          <li key={id}>
            <Link href={`/products/${id}`}>/Product {id}</Link>
          </li>
        ))}
      </ul>
    </>
  );
}
