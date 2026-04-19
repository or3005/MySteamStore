
import { User } from "../Model/User";

const BASE_URL_USER = "http://localhost:5210/api/users";



async function userRequest<T>(Url: string, options: RequestInit,): Promise<T> {
  try {
    const response = await fetch(`${BASE_URL_USER}${Url}`, options);
    if (!response.ok) {
      throw new Error((`Request failed: ${response.status}`));
    }
    const json = await response.json();
    return json;
  } catch (error) {
    console.error(error);
    throw error;
  }

}

export async function getAllusers() {

  return await userRequest<User[]>("/all-users", {
    method: "GET"
  })

}

export async function Register(name: string, password: string) {
  return await userRequest<User>(
    `/register?name=${encodeURIComponent(name)}&password=${encodeURIComponent(password)}`, {
    method: "POST",
  })
}



export async function login(name: string, password: string) {
  return await userRequest<User>(
    `/login?name=${encodeURIComponent(name)}&password=${encodeURIComponent(password)}`, {
    method: "POST",
  })
}


export async function GetUser(id: string) {
  return await userRequest<User>(
    `/${id}`, {
    method: "GET",
  }
  )
}