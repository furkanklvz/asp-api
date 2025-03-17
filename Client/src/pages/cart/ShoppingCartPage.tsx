import { useEffect, useState } from "react";
import requests from "../../api/requests";
import { TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, Typography, IconButton, CircularProgress } from "@mui/material";
import { ICart } from "../../model/ICart";
import { DeleteOutline} from "@mui/icons-material";

export default function AhoppingCartPage() {
    const [loading, setLoading] = useState(true);
    const [cart, setCart] = useState<ICart>();

    useEffect(() => {
        requests.Cart.get()
            .then(cart => setCart(cart))
            .catch(error => console.log(error))
            .finally(() => setLoading(false));
    }, [])

    if( loading) return <CircularProgress/>;
    return (
        <>
            <Typography variant="h3" style={{margin:5}}>Shopping Cart</Typography>

            <TableContainer sx={{ marginTop: 5 }} component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell></TableCell>
                            <TableCell>Product</TableCell>
                            <TableCell align="right">Price</TableCell>
                            <TableCell align="right">Quantity</TableCell>
                            <TableCell align="right">Total Price)</TableCell>
                            <TableCell align="right"></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {cart?.cartItems.map((cartItem) => (
                            <TableRow
                                key={cartItem.productId}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                            >
                                <TableCell component="th" scope="row">
                                    <img src={`http://localhost:5077/images/${cartItem.productImageUrl}`} style={{ height: 60 }} />
                                </TableCell>
                                <TableCell component="th" scope="row">{cartItem.productName}</TableCell>
                                <TableCell align="right">{cartItem.productPrice} ₺</TableCell>
                                <TableCell align="right">{cartItem.quantity}</TableCell>
                                <TableCell align="right">{(cartItem.productPrice * cartItem.quantity)} ₺</TableCell>
                                <TableCell align="right">
                                    <IconButton color="error">{<DeleteOutline/>}</IconButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}