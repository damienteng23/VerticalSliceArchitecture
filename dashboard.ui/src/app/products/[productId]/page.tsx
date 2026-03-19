import Link from "next/link";

export default async function ProductDetails({
  params,
}: {
  params: Promise<{ productId: string }>;
}) {
  const { productId } = await params;

  return (
    <>
      <h1>details about product {productId}</h1>
      <p>some details about the product_{productId}</p>
      <Link href={`/products`}>/Back to Products</Link>
    </>
  );
}
