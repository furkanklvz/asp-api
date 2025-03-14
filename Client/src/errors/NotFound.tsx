import { Button, Container, Divider, Typography } from "@mui/material";
import { NavLink, useLocation } from "react-router";

export default function NotFound() {

    const { state } = useLocation();

    return (
        <Container>
            {
                state?.error ? (
                    <>
                        <Typography variant="h3" gutterBottom>{state.error.title} - {state.status}</Typography>
                    </>
                ) : (
                    <Typography>Server Error</Typography>
                )
            }
            <Divider />
            <Button variant="contained" component={NavLink} to={"/catalog"} sx={{mt:2}}>Continue Shopping</Button>
        </Container>
    )
}