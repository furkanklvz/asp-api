import { useEffect, useState } from "react"
import { IProduct } from "../../model/IProduct";
import ProductList from "./ProductList";
import { CircularProgress } from "@mui/material";
import requests from "../../api/requests";

export default function CatalogPage() {

    
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        requests.Catalog.list()
            .then(data => setProducts(data))
            .catch(error => console.log(error))
            .finally(()=> setLoading(false));

        console.log("fethced data")
    }, []);

    const [products, setProducts] = useState<IProduct[]>([]);

    if(loading) return <CircularProgress/>
    return (
    <ProductList products ={products}/>
  );
}