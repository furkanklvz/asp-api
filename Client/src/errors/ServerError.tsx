import { Button, Container, Divider, Typography } from "@mui/material";
import { NavLink, useLocation } from "react-router";

export default function ServerError(){

    const { state } = useLocation();

    return(
        <Container>
            {
                state?.error ? (
                    <>
                        <Typography variant="h3" gutterBottom>{state.error.title} - {state.status}</Typography>
                        <Divider/>
                        <Typography variant="body2">{state.error.detail || "unknown error"}</Typography>
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