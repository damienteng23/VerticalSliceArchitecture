import { notFound } from "next/dist/client/components/navigation";

export default async function ProductReview({
  params,
}: {
  params: Promise<{ productId: string; reviewId: string }>;
}) {
  const { productId, reviewId } = await params;

  if (parseInt(reviewId) > 1000) {
    return notFound();
  }

  return (
    <h1>
      details about product {productId} and review {reviewId}
    </h1>
  );
}
