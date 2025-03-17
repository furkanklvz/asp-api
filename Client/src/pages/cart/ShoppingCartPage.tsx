import { useEffect, useState } from "react";
import requests from "../../api/requests";

export default function AhoppingCartPage() {
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        requests.Cart.get()
        .then(cart => console.log(cart))
        .catch(error => console.log(error))
        .finally(() => setLoading(false));
    },[])

    return (<></>);
}