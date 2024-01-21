import { Photo } from "./profile";

export interface User {
  username: string;
  displayName: string;
  token: string;
  image?: string;
  bio?: string;
  photos?: Photo[];
}

export interface UserFormValues {
  username?: string;
  displayName?: string;
  password: string;
  email: string;
}
