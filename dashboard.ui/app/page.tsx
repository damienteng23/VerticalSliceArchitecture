import { redirect } from "next/navigation";

export default function Home() {
  redirect("/backtesting/moving-average");
}
