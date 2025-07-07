export default function ProductDetails({
    params,
}: {
    params: { productId: string };
}) {
    return <h1> Product Id {params.productId} </h1>;
}
