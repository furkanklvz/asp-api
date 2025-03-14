
import Button from '@mui/material/Button';

export default function ButtonUsage(prop: any) {
  return <Button variant="contained" 
    onClick={prop.onClick}>Hello world</Button>;
}