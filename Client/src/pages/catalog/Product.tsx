import { AddShoppingCart, OpenInNew } from "@mui/icons-material";
import { Button, Card, CardActions, CardContent, CardMedia, IconButton, Typography } from "@mui/material";
import { Link } from "react-router";
import requests from "../../api/requests";
import { useState } from "react";
import { toast } from "react-toastify";

export default function Product(props: any) {

  const [loading, setLoading] = useState(false);

  function AddToCart() {

    setLoading(true);

    requests.Cart.addItem(props.product.productId)
      .then(cart => {
        console.log(cart)
        toast.success("Item added to cart.")
      })
      .catch(error => console.log(`error: ${error}`))
      .finally(() => setLoading(false));
  }

  return (
    <Card variant="outlined">
      <CardMedia sx={{ height: 160, backgroundSize: "contain" }} image={`http://localhost:5077/images/${props.product.imageUrl}`} />
      <CardContent>
        <Typography gutterBottom variant="h6" component="h2" color="text.secondary">{props.product.name}</Typography>
        <Typography variant="body2" color="secondary">{(props.product.price / 100).toFixed(2)} ₺</Typography>
      </CardContent>
      <CardActions>
        <IconButton color="primary" onClick={AddToCart} loading={loading}>
          <AddShoppingCart />
        </IconButton>
        <Button component={Link} to={`/catalog/${props.product.productId}`} variant="contained" startIcon={<OpenInNew />} color="success">View
        </Button>
      </CardActions>
    </Card>
  )
}