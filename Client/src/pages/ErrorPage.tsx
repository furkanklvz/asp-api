import { Alert, AlertTitle, Button, Container, List, ListItem, ListItemText, Stack } from "@mui/material";
import requests from "../api/requests";
import { useState } from "react";

export default function ErrorPage() {

    const [validationErrors, setValidationErrors] = useState<string[]>([]);

    function getValidationErrors() {
        requests.Errors.getValidationError()
            .then(() => console.log("no validation errors"))
            .catch(errors => setValidationErrors(errors))
    }

    return (
        <Container sx={{margin:3}}>
            {
                validationErrors.length > 0 && (
                    <Alert severity="error" sx={{ mb: 2 }}>
                        <AlertTitle>Validation Errors</AlertTitle>
                        <List>
                            {
                                validationErrors.map((error, index) => (
                                    <ListItem key={index}>
                                        <ListItemText>{error}</ListItemText>
                                    </ListItem>
                                ))
                            }
                        </List>
                    </Alert>
                )
            }
            <Stack margin={5} direction={"row"}>
                <Button sx={{ mr: 2 }} variant="contained" onClick={() => requests.Errors.get400Error().catch(error => console.log(error))}>
                    Try 400</Button>
                <Button sx={{ mr: 2 }} variant="contained" onClick={() => requests.Errors.get401Error().catch(error => console.log(error))}>
                    Try 401</Button>
                <Button sx={{ mr: 2 }} variant="contained" onClick={() => requests.Errors.get404Error().catch(error => console.log(error))}>
                    Try 404</Button>
                <Button sx={{ mr: 2 }} variant="contained" onClick={() => requests.Errors.get500Error().catch(error => console.log(error))}>
                    Try 500</Button>
                <Button sx={{ mr: 2 }} variant="contained" onClick={getValidationErrors}>
                    Try Validation Error</Button>
            </Stack>
        </Container>
    )
}