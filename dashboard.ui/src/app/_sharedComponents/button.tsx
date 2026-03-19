"use client";

export default function MyButton() {
  const router = useRouter();
  const handleClick = () => {
    alert("Button clicked!");
  };

  return (
    <button
      onClick={handleClick}
      style={{
        padding: "10px 20px",
        backgroundColor: "blue",
        borderRadius: "5px",
      }}
    >
      Click Me
    </button>
  );
}
