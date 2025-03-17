import { CircularProgress, Divider, Grid2, Table, TableBody, TableCell, TableRow, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { IProduct } from "../../model/IProduct";
import requests from "../../api/requests";

export default function ProductDetailsPage() {

    const { id } = useParams();
    const [product, setProduct] = useState<IProduct>();
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        id && requests.Catalog.details(parseInt(id))
            .then(data => setProduct(data))
            .catch(error => console.info(error))
            .finally(() => setLoading(false));
    }, [id]);


    if (loading) {
        return (<CircularProgress />)
    }

    return (
        product && <Grid2 container spacing={2}>
            <Grid2 size={{ xs: 12, sm: 6, md: 5, lg: 4, xl: 4 }}>
                <img src={`http://localhost:5077/images/${product.imageUrl}`} style={{ width: "100%" }}></img>
            </Grid2>
            <Grid2 size={{ xs: 12, sm: 6, md: 7, lg: 8, xl: 8 }}>
                <Typography variant="h2">{product.name}</Typography>
                <Divider/>
                <Typography variant="h4" color="secondary">{(product.price/100).toFixed(2)} ₺</Typography>
                <Table>
                    <TableBody>
                        <TableRow>
                            <TableCell>Name</TableCell>
                            <TableCell>{product.name}</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>Description</TableCell>
                            <TableCell>{product.description}</TableCell>
                        </TableRow>
                        <TableRow>
                            <TableCell>Stock</TableCell>
                            <TableCell>{product.stock}</TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </Grid2>
        </Grid2>
    );
}