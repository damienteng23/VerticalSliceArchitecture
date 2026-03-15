export default async function ProductReview(
    { params }: { params: Promise<{ productId: string; reviewId: string }> }
) {

  const { productId, reviewId } = await params;

  return (
    <h1>details about product {productId} and review {reviewId}</h1>
  )
}