export interface ICart {
    cartId: number
    customerId: string
    cartItems: ICartItem[]
  }

  export interface ICartItem {
    productId: number
    productName: string
    productPrice: number
    productImageUrl: string
    quantity: number
  }
