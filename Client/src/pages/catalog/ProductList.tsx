import { Grid2, Typography } from "@mui/material";
import Product from "./Product";

export default function ProductList(props: any) {

  return (
    <>
      <Typography variant="h6" margin={5}>Product List</Typography>
      <Grid2 container spacing={3}>
        {props.products.map((p: any) => (
          p.isActive && <Grid2 key={p.productId} size={{ xs: 6, md: 4, lg: 3 }}>
            <Product product={p} />
          </Grid2>
        ))}
      </Grid2>
    </>
  )
}