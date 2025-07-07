export default async function ReviewDetails({ params }: {
    params: {
        productId: string;
        reviewId: string;
    };
}) {
    // Await the params directly
    const { productId, reviewId } = await params;

    return (
        <h1>
            Review {reviewId} for product {productId}
        </h1>
    );
}
