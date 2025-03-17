import { Person2Rounded, ShoppingCart } from "@mui/icons-material";
import { AppBar, Badge, Box, Button, IconButton, Stack, Toolbar, Typography } from "@mui/material";
import { Link, NavLink } from "react-router";

const links = [
  { title: "Home", to: "" },
  { title: "Contact", to: "/contact" },
  { title: "About", to: "/about" },
  { title: "Catalog", to: "/catalog" },
  { title: "Error", to: "/error-test" },
]
const navStyles = {
  color:"inherit",
  textDecoration: "none",
  "&:hover":{
    color:"text.primary"
  },
  "&.active":{
    color:"error.main"
  }
} 

export default function Header() {
  return (
    <AppBar position="static">
      <Toolbar sx={{justifyContent:"space-between", flex:"center"}}>
        <Box sx={{display:"flex", alignItems:"center"}}>
          <Typography variant="h6">E-Commerce</Typography>
          <Stack direction="row">
            {links.map(link =>
              <Button key={link.to} component={NavLink} sx={navStyles} to={link.to}>{link.title}</Button>
            )}
          </Stack>
        </Box>
        <Box sx={{display:"flex", alignItems:"center"}}>
          <IconButton component={Link} to="/cart" size="large" edge="start" color="inherit">
            <Badge badgeContent="3" color="secondary">
              <ShoppingCart/>
            </Badge>
          </IconButton>
          <IconButton color="inherit">
            <Person2Rounded/>
          </IconButton>
        </Box>
      </Toolbar>
    </AppBar>
  )
}