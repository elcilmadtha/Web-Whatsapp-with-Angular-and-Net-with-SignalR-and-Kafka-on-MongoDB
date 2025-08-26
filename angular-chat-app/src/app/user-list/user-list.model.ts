export interface UsersDto {
  id: string;
  name: string;
  email: string;
  username: string;
}

export interface GetUsersResponse {
  users: UsersDto[];
  isSuccess: boolean;
  errorMessage: string[];
  validationMessage: string[];
}
